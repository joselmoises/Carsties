using System;
using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _context;
    private readonly IMapper _mapper;
    public AuctionsController(AuctionDbContext context, IMapper mapper)
    {
        _mapper = mapper;
        _context = context;

    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
    {
        var auctions = await _context.Auctions
        .Include(i => i.Item)
        .OrderBy(m => m.Item.Make)
        .ToListAsync();
        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
        .Include(i => i.Item)
        .FirstOrDefaultAsync(a => a.Id == id);
        if (auction == null)
        {
            return NotFound();
        }
        return _mapper.Map<AuctionDto>(auction);
    }

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto createAuctionDto)
    {
        var auction = _mapper.Map<Auction>(createAuctionDto);
        auction.Id = Guid.NewGuid();
        _context.Auctions.Add(auction);
        await _context.SaveChangesAsync();
        var auctionDto = _mapper.Map<AuctionDto>(auction);
        return CreatedAtAction(nameof(GetAuctionById), new { id = auction.Id }, _mapper.Map<AuctionDto>(auction));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }
        _mapper.Map(updateAuctionDto, auction);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {
            return Ok();
        }
        return BadRequest("Failed to update the auction");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _context.Auctions.FindAsync(id);
        if (auction == null)
        {
            return NotFound();
        }
        _context.Auctions.Remove(auction);
        var result = await _context.SaveChangesAsync() > 0;
        if (result)
        {

            return Ok();
        }
        return BadRequest("Failed to delete the auction");
    }

}
