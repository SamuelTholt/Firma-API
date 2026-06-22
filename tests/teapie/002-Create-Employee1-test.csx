await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("EmployeeId uložený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("EmployeeId", (int)body.Id);
});

await tp.Test("Údaje zamestnanca 1 sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Ján", (string)body.FirstName);
    Equal("Novák", (string)body.LastName);
    Equal("Ing.", (string)body.Title);
});
