await tp.Test("Status je 200 OK", async () =>
{
    Equal(200, (int)tp.Response.StatusCode);
});

await tp.Test("Správne oddelenie vrátené", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal(tp.GetVariable<int>("DepartmentId"), (int)body.Id);
    Equal("DEP-DEV", (string)body.Code);
});
