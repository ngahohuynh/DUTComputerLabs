using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface IComputerLabService : IRepository<ComputerLab>
    {
        PagedList<ComputerLab> GetComputerLabs(LabParams labParams);

        ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab);

        ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab);

        PagedList<ComputerLab> SearchComputerLabsForBooking(LabParams labParams);
    }

    public class ComputerLabService : Repository<ComputerLab>, IComputerLabService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ComputerLabService(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public PagedList<ComputerLab> GetComputerLabs(LabParams labParams)
        {
            var labs = _context.ComputerLabs.Where(l => l.OwnerId == labParams.OwnerId).AsQueryable();
            return PagedList<ComputerLab>.Create(labs, labParams.PageNumber, labParams.PageSize);
        }

        public ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab)
        {
            var labToAdd = _mapper.Map<ComputerLab>(computerLab);

            labToAdd.Owner = _context.Users.Find(computerLab.OwnerId);

            return _mapper.Map<ComputerLabForList>(labToAdd);
        }

        public ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab)
        {
            var labToUpdate = GetById(id);

            _mapper.Map(computerLab, labToUpdate);

            return _mapper.Map<ComputerLabForList>(GetById(id));
        }

        public PagedList<ComputerLab> SearchComputerLabsForBooking(LabParams labParams)
        {
            var labs = _context.ComputerLabs.Include(l => l.Bookings)
                        .Where(l => CheckValidComputerLab(l.Bookings, labParams.BookingDate, labParams.StartAt, labParams.EndAt));
            return PagedList<ComputerLab>.Create(labs, labParams.PageNumber, labParams.PageSize);
        }

        private bool CheckValidComputerLab(ICollection<Booking> bookings, DateTime bookingDate, int startAt, int endAt)
        {
            return bookings.Any(b => b.BookingDate.Date == bookingDate.Date && 
                                    (b.StartAt > endAt || b.EndAt < startAt ));
        }
    }
}