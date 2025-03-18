using Crazy_Musicians.Models;
using Microsoft.AspNetCore.Mvc;

namespace Crazy_Musicians.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusiciansController : ControllerBase
    {
        private static List<Musician> _musicians = new List<Musician>()
        {
            new Musician { Id = 1, Name = "Ahmet Çalgı", Profession = "Famous Instrument Player", FunFact = "Always plays wrong notes, but very fun" },
            new Musician { Id = 2, Name = "Zeynep Melodi", Profession = "Popular Melody Writer", FunFact = "Her songs are often misunderstood but very popular" },
            new Musician { Id = 3, Name = "Cemil Akor", Profession = "Crazy Chordist", FunFact = "Frequently changes chords, but surprisingly talented" },
            new Musician { Id = 4, Name = "Fatma Nota", Profession = "Surprise Note Producer", FunFact = "Always prepares surprises while producing notes" },
            new Musician { Id = 5, Name = "Hasan Ritim", Profession = "Rhythm Monster", FunFact = "Plays every rhythm in his own style, never fits but is funny" },
            new Musician { Id = 6, Name = "Elif Armoni", Profession = "Harmony Master", FunFact = "Sometimes plays harmonies incorrectly, but very creative" },
            new Musician { Id = 7, Name = "Ali Perde", Profession = "Pitch Adapter", FunFact = "Plays every pitch differently, always surprising" },
            new Musician { Id = 8, Name = "Ayşe Rezonans", Profession = "Resonance Expert", FunFact = "An expert in resonance, but sometimes makes a lot of noise" },
            new Musician { Id = 9, Name = "Murat Ton", Profession = "Tone Enthusiast", FunFact = "Tone variations are sometimes funny, but quite interesting" },
            new Musician { Id = 10, Name = "Selin Akor", Profession = "Chord Wizard", FunFact = "Creates a magical atmosphere when changing chords" }
        };

        [HttpGet]
        public IActionResult GetAll()
        {
            var response = _musicians.Select(x => new MusicianListResponse
            {
                Id = x.Id,
                Name = x.Name,
                Profession = x.Profession,
                FunFact = x.FunFact
            }).ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
                return NotFound();


            var response = new MusicianListResponse()
            {
                Id = musician.Id,
                Name = musician.Name,
                Profession = musician.Profession,
                FunFact = musician.FunFact
            };

            return Ok(response);
        }

        [HttpPost]
        public IActionResult PostMusician(MusicianPostRequest request)
        {

            var newMusician = new Musician()
            {
                Id = _musicians.Max(x => x.Id) + 1,
                Name = request.Name,
                Profession = request.Profession,
                FunFact = request.FunFact
            };

            _musicians.Add(newMusician);

            return CreatedAtAction(nameof(Get), new { id = newMusician.Id }, newMusician);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateMusician(int id, MusicianUpdateResponse response)
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
                return NotFound();

            musician.Name = response.Name;
            musician.Profession = response.Profession;
            musician.FunFact = response.FunFact;

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateFunFact(int id, MusicianFunFactUpdateRequest request)
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
                return NotFound();

            musician.FunFact = request.FunFact;

            return Ok(request);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var musician = _musicians.FirstOrDefault(x => x.Id == id);

            if (musician == null)
                return NotFound();

            _musicians.Remove(musician);

            return Ok();
        }

        [HttpGet("search")]
        public IActionResult SearchMusicians([FromQuery] string name)
        {
            var result = _musicians
                .Where(m => string.IsNullOrEmpty(name) || m.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(x => new MusicianListResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Profession = x.Profession,
                    FunFact = x.FunFact
                })
                .ToList();

            if (!result.Any())
                return NotFound("No musicians found with the given name.");

            return Ok(result);
        }

    }
}
