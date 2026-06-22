await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("EmployeeId2 uložený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("EmployeeId2", (int)body.Id);
});

await tp.Test("Údaje zamestnanca 2 sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Eva", (string)body.FirstName);
    Equal("Kováčová", (string)body.LastName);
    Equal("Mgr.", (string)body.Title);
});
