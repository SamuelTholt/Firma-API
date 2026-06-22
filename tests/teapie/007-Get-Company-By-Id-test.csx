await tp.Test("Status je 200 OK", async () =>
{
    Equal(200, (int)tp.Response.StatusCode);
});

await tp.Test("Správna firma vrátená", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal(tp.GetVariable<int>("CompanyId"), (int)body.Id);
    Equal("TF-001", (string)body.Code);
});
