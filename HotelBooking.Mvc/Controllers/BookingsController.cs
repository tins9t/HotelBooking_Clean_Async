using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HotelBooking.Core;
using HotelBooking.Mvc.Models;

namespace HotelBooking.Mvc.Controllers
{
    public class BookingsController : Controller
    {
        private IRepository<Booking> bookingRepository;
        private IRepository<Customer> customerRepository;
        private IRepository<Room> roomRepository;
        private IBookingManager bookingManager;
        private IBookingViewModel bookingViewModel;

        public BookingsController(IRepository<Booking> bookingRepos, IRepository<Room> roomRepos,
            IRepository<Customer> customerRepos, IBookingManager manager, IBookingViewModel viewModel)
        {
            bookingRepository = bookingRepos;
            roomRepository = roomRepos;
            customerRepository = customerRepos;
            bookingManager = manager;
            bookingViewModel = viewModel;
        }

        // GET: Bookings
        public IActionResult Index(int? id)
        {
            bookingViewModel.YearToDisplay = (id == null) ? DateTime.Today.Year : id.Value;
            return View(bookingViewModel);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking = await bookingRepository.GetAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = 
                new SelectList(await customerRepository.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartDate,EndDate,CustomerId")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                bool created = await bookingManager.CreateBooking(booking);

                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewData["CustomerId"] = new SelectList(
                await customerRepository.GetAllAsync(), "Id", "Name", booking.CustomerId);
            ViewBag.Status = "The booking could not be created. There were no available room.";
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking = await bookingRepository.GetAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(
                await customerRepository.GetAllAsync(), "Id", "Name", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(
                await roomRepository.GetAllAsync(), "Id", "Description", booking.RoomId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartDate,EndDate,IsActive,CustomerId,RoomId")] Booking booking)
        {
            if (id != booking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await bookingRepository.EditAsync(booking);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await bookingRepository.GetAsync(booking.Id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList(
                await customerRepository.GetAllAsync(), "Id", "Name", booking.CustomerId);
            ViewData["RoomId"] = new SelectList(
                await roomRepository.GetAllAsync(), "Id", "Description", booking.RoomId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Booking booking = await bookingRepository.GetAsync(id.Value);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await bookingRepository.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
