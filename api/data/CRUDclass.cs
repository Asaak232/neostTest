using BaseSpace.Data;
using BaseSpace.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUD.actions
{
    public class CRUDClass
    {
        private readonly DataContext current_context;

        public CRUDClass(DataContext context)
        {
            current_context = context;
        }

        public async Task<IResult> CreatePerson(Person request)
        {
            try
            {
                var person = new Person
                {
                    Name = request.Name,
                    DisplayName = request.DisplayName,
                    Skills = request.Skills?.Select(s => new Skill
                    {
                        Name = s.Name,
                        Level = s.Level
                    }).ToList()
                };

                current_context.Persons.Add(person);

                await current_context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (DbUpdateException e)
            {
                string errorMessage = $"An error occurred: {e.Message}|" +
                                    $"Inner Exception: {e.InnerException.Message}|" +
                                    $"Stack Trace: {e.StackTrace}";

                return Results.BadRequest(errorMessage);
            }

        }

        public async Task<Person> GetPersonById(long id)
        {
            Person result = await current_context.Persons
                    .Include(p => p.Skills)
                    .FirstOrDefaultAsync(p => p.Id == id);
                    //Unable to make linq to work here and afterwards.

            return result;
        }
        public async Task<IResult> GetEveryone()
        {
            List<Person> result =  await current_context.Persons
                        .Include(p => p.Skills.OrderBy(s => s.Name))
                        .ToListAsync();

            return Results.Ok(result);
        }

        public async Task<IResult> UpdatePerson(long id, Person request)
        {

            Person person = await GetPersonById(id);

            if (person == null)
            {
                return Results.NotFound("Person not found");
            }
            else try
            {
                person.Name = request.Name;
                person.DisplayName = request.DisplayName;

                if (request.Skills != null)
                {
                    current_context.Skills.RemoveRange(person.Skills);

                    foreach (Skill skill in request.Skills)
                    {
                        person.Skills.Add(new Skill
                        {
                            Name = skill.Name,
                            Level = skill.Level
                        });
                    }
                }

                await current_context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (DbUpdateException e)
            {
                string errorMessage = $"An error occurred: {e.Message}|" +
                                    $"Inner Exception: {e.InnerException.Message}|" +
                                    $"Stack Trace: {e.StackTrace}";

                return Results.BadRequest(errorMessage);
            }
        }

        public async Task<IResult> DeletePerson(long id)
        {
            Person person = await GetPersonById(id);

            if (person == null)
            {
                return Results.NotFound("Person not found");
            }
            else try
            {
                current_context.Persons.Remove(person);

                await current_context.SaveChangesAsync();

                return Results.Ok();
            }
            catch (DbUpdateException e)
            {
                string errorMessage = $"An error occurred: {e.Message}|" +
                                    $"Inner Exception: {e.InnerException.Message}|" +
                                    $"Stack Trace: {e.StackTrace}";

                return Results.BadRequest(errorMessage);
            }

        }

    }
}

