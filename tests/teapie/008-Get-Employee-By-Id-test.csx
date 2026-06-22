await tp.Test("Status je 200 OK", async () =>
{
    Equal(200, (int)tp.Response.StatusCode);
});

await tp.Test("Správny zamestnanec vrátený", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal(tp.GetVariable<int>("EmployeeId"), (int)body.Id);
    Equal("Ján", (string)body.FirstName);
});
