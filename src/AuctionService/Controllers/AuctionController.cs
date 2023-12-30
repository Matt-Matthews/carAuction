using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers
{
    [ApiController]
    [Route("api/auctions")]
    public class AuctionController : ControllerBase
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionController(AuctionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions.Include(x => x.Item).OrderBy(x => x.Item.Make).ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

            if(auction == null) return NotFound();

            return _mapper.Map<AuctionDto>(auction);
        }

        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Seller = "test";

            _context.Auctions.Add(auction);

            var results = await _context.SaveChangesAsync() > 0;

            if(!results) return BadRequest("Could not save changes to the DB");

            return CreatedAtAction(nameof(GetAuctionById), new {auction.Id}, _mapper.Map<AuctionDto>(auction));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAuction(Guid id, UpdateauctionDto updateauctionDto)
        {
            var auction = await _context.Auctions.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id);

            if(auction == null) return NotFound();

            auction.Item.Make = updateauctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateauctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateauctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateauctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateauctionDto.Year ?? auction.Item.Year;

            var results = await _context.SaveChangesAsync() > 0;

            if(results) return Ok();

            return BadRequest("Problem saving changes");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            if(auction == null) return NotFound();

            //TODO: Check seller = username

            _context.Auctions.Remove(auction);

            var results = await _context.SaveChangesAsync() > 0;

            if(!results) return BadRequest("Could not update DB");

            return Ok();
        }
    }
}