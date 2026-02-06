using ArchitecturePractice.Repositories;
using ArchitecturePractice.Services;
using Serilog;

// 先配置簡易Logger輸出到控制台，因為一開始還沒讀到appsetting檔案
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("應用程式正在啟動...");

    var builder = WebApplication.CreateBuilder(args);
    
    // 替換內建ILogger
    builder.Host.UseSerilog((context, services, configuration) => configuration
           .ReadFrom.Configuration(context.Configuration) // 從 appsettings.json 讀取配置
           .ReadFrom.Services(services)                   // 擴充性，可以讓 Serilog 從 DI 取得資訊
           .Enrich.FromLogContext()); // 會自動幫Log加入TraceId，不需要手動加入，也可以加入其他參數客製化Log

    // Add services to the container.
    builder.Services.AddControllersWithViews();

    // 取得資料庫連線字串
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("找不到資料庫連線字串。");

    // 註冊管理
    builder.Services.AddService()
                    .AddRepository(connectionString);

    var app = builder.Build();

    // 進入判斷後，開始分流錯誤處理
    if (app.Environment.IsDevelopment())
    {
        app.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), appBuilder =>
        {
            appBuilder.UseExceptionHandler("/api/error");
        });

        app.UseWhen(context => !context.Request.Path.StartsWithSegments("/api"), appBuilder =>
        {
            appBuilder.UseExceptionHandler("/error/error");

            // 處理400~599的狀態碼，主要是404、403，會將錯誤碼傳到Action去做判斷
            appBuilder.UseStatusCodePagesWithReExecute("/error/error/{0}");
        });

        app.UseHsts();
    }

    // 啟用 Serilog 的 HTTP 請求日誌
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=ExportReport}/{action=ExportPage}/{id?}")
        .WithStaticAssets();


    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "應用程式啟動失敗！");
}
finally
{
    // 程式結束前，確保把記憶體中的 Log 寫入檔案或傳送到 Seq
    Log.CloseAndFlush();
}