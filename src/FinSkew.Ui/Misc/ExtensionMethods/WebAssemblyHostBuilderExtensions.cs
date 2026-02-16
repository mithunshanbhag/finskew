namespace FinSkew.Ui.Misc.ExtensionMethods;

public static class WebAssemblyHostBuilderExtensions
{
    extension(WebAssemblyHostBuilder builder)
    {
        public WebAssemblyHostBuilder ConfigureApp()
        {
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            return builder;
        }

        public WebAssemblyHostBuilder ConfigureServices()
        {
            // automapper
            builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MapperProfile>(); });

            // mediatr
            builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()); });

            // named http clients

            // mudblazor
            builder.Services.AddMudServices();

            // auth

            // services
            builder.Services
                .AddSingleton<ICalculator<SimpleInterestInputViewModel, SimpleInterestResultViewModel>, SimpleInterestCalculator>()
                .AddSingleton<ICalculator<CompoundInterestInputViewModel, CompoundInterestResultViewModel>, CompoundInterestCalculator>()
                .AddSingleton<ICalculator<SipInputViewModel, SipResultViewModel>, SipCalculator>()
                .AddSingleton<ICalculator<StepUpSipInputViewModel, StepUpSipResultViewModel>, StepUpSipCalculator>()
                .AddSingleton<ICalculator<LumpsumInputViewModel, LumpsumResultViewModel>, LumpsumCalculator>()
                .AddSingleton<ICalculator<SwpInputViewModel, SwpResultViewModel>, SwpCalculator>()
                .AddSingleton<ICalculator<CagrInputViewModel, CagrResultViewModel>, CagrCalculator>();

            // repositories
            builder.Services
                .AddTransient<ICsvRepository1, CsvRepository1>();

            return builder;
        }
    }
}