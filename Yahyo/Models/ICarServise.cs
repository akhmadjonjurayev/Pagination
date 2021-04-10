using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Yahyo.Models
{
    public interface ICarServise
    {
        Task<Response> CreateAsync(Car car, List<IFormFile> files);
        Task<IEnumerable<Car>> GetCarAsync();
        Task DeleteCarAsync(int id);
        Task<Car> GetCarAsync(int id);
        Task<IEnumerable<Car>> GetCarPaginationAsync(int length, int index);
        Task<int> GetCarCountAsync();
    }
    public class CarService : ICarServise
    {
        private readonly IWebHostEnvironment _web;
        private readonly DbClass _db;

        public CarService(DbClass db, IWebHostEnvironment webHost)
        {
            _web = webHost;
            _db = db;
        }
        public async Task<Response> CreateAsync(Car car, List<IFormFile> files)
        {
            if (files.Count() == 0)  return new Response() { IsFile = true, IsSuccsess = false, Message = "Please enter the car's photo" };
            else if (files.Count() > 6) return new Response() { IsSuccsess = false, IsFile = true, Message = "Please do not enter photo more than 6" };
            foreach(var file in files)
            {
                if (Path.GetExtension(file.FileName).ToLower() != ".jpg" && Path.GetExtension(file.FileName).ToLower() != ".png")
                    return new Response() { IsFile = true, IsSuccsess = false, Message = "Please enter only photo" };
            }
            await _db.Car.AddAsync(car);
            await _db.SaveChangesAsync();
            foreach(var file in files)
            {
                var photo = new Photo() { CarId = car.Id };
                var image = Path.Combine(_web.WebRootPath, "Photos", file.FileName);
                using(var stream=new FileStream(image, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    photo.LocationName = file.FileName;
                    await _db.Photos.AddAsync(photo);
                    stream.Close();
                }
            }
            await _db.SaveChangesAsync();
            return new Response() { IsFile = false, IsSuccsess = true, Message = "Car saved successfully" };
        }

        public async Task DeleteCarAsync(int id)
        {
            var photos = await _db.Photos.Where(i => i.CarId == id).ToListAsync();
            foreach(var photo in photos)
            {
                _db.Photos.Remove(photo);
            }
            var car = await _db.Car.FindAsync(id);
            _db.Car.Remove(car);
            _db.SaveChanges();
        }

        public async Task<IEnumerable<Car>> GetCarAsync()
        {
            var cars = await _db.Car.Where(i => true).Include(i => i.Photos).OrderByDescending(i => i.Id).ToListAsync();
            return cars;
        }

        public async Task<Car> GetCarAsync(int id)
        {
            return await _db.Car.Where(x => x.Id == id)
                                 .Include(z => z.Photos).FirstOrDefaultAsync();
        }

        public async Task<int> GetCarCountAsync()
        {
            return await _db.Car.CountAsync();
        }

        public async Task<IEnumerable<Car>> GetCarPaginationAsync(int length, int index)
        {
            if (index < 1) index = 1;
            else
            {
                int count = await _db.Car.CountAsync();
                if (index * length > count) index = count / length + 1;
            }
            return await _db.Car.Include(i => i.Photos)
                .Skip(length * (index - 1))
                .Take(length)
                .ToListAsync();
        }
    }
}
