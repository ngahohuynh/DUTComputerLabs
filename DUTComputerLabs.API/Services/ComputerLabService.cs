using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DUTComputerLabs.API.Data;
using DUTComputerLabs.API.Dtos;
using DUTComputerLabs.API.Exceptions;
using DUTComputerLabs.API.Helpers;
using DUTComputerLabs.API.Models;
using DUTComputerLabs.API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DUTComputerLabs.API.Services
{
    public interface IComputerLabService : IRepository<ComputerLab>
    {
        PagedList<ComputerLab> GetComputerLabs(LabParams labParams);

        ComputerLabForDetailed GetComputerLab(int id);

        ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab);

        ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab);

        IEnumerable<ComputerLabForList> SearchComputerLabsForBooking(LabParams labParams);
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

        public ComputerLabForDetailed GetComputerLab(int id)
        {
            var lab = _context.ComputerLabs.Include(l => l.Owner).FirstOrDefault(l => l.Id == id)
                ?? throw new BadRequestException("Phòng máy này không tồn tại");
            return _mapper.Map<ComputerLabForDetailed>(lab);
        }

        public ComputerLabForList AddComputerLab(ComputerLabForInsert computerLab)
        {
            if(_context.ComputerLabs.Any(l => string.Equals(l.Name, computerLab.Name)))
            {
                throw new BadRequestException("Phòng máy này đã tồn tại");
            }

            var labToAdd = _mapper.Map<ComputerLab>(computerLab);

            labToAdd.Owner = _context.Users.Find(computerLab.OwnerId);
            
            Add(labToAdd);

            return _mapper.Map<ComputerLabForList>(labToAdd);
        }

        public ComputerLabForList UpdateComputerLab(int id, ComputerLabForInsert computerLab)
        {
            var labToUpdate = GetById(id);

            _mapper.Map(computerLab, labToUpdate);
            _context.SaveChanges();

            return _mapper.Map<ComputerLabForList>(GetById(id));
        }

        public IEnumerable<ComputerLabForList> SearchComputerLabsForBooking(LabParams labParams)
        {
            var labs = _context.ComputerLabs.Include(l => l.Bookings)
                        // .Where(l => CheckValidComputerLab(l.Bookings, labParams.BookingDate, labParams.StartAt, labParams.EndAt));
                        .Where(l => !l.Bookings.Any(b => b.BookingDate.Date == labParams.BookingDate.Date
                                    && ( (b.EndAt >= labParams.StartAt && b.StartAt <= labParams.EndAt)
                                    || (b.StartAt >= labParams.StartAt && b.StartAt <= labParams.EndAt) ) ));
            return _mapper.Map<IEnumerable<ComputerLabForList>>(labs);
        }

        private bool CheckValidComputerLab(ICollection<Booking> bookings, DateTime bookingDate, int startAt, int endAt)
        {
            return bookings.Any(b => b.BookingDate.Date == bookingDate.Date && 
                                    (b.StartAt > endAt || b.EndAt < startAt ));
        }
    }
}