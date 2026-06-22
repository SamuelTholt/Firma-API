await tp.Test("Status je 201 Created", async () =>
{
    Equal(201, (int)tp.Response.StatusCode);
});

await tp.Test("Response obsahuje ID", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    NotNull((object)body.Id);
    tp.SetVariable("CompanyId", (int)body.Id);
});

await tp.Test("Name a Code sa zhodujú", async () =>
{
    dynamic body = await tp.Response.GetBodyAsExpandoAsync();
    Equal("Testovacia Firma s.r.o.", (string)body.Name);
    Equal("TF-001", (string)body.Code);
});
