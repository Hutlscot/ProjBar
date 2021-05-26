namespace ProjBar
{
    using System;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// вьюмодель главного окна
    /// </summary>
    public class MainVM
    {
        /// <summary>
        /// команда отмены
        /// </summary>
        private Command cancelCommand;

        /// <summary>
        /// непонятная штука для отмена операций
        /// </summary>
        private CancellationTokenSource cancelTokenSource;

        /// <summary>
        /// команда скачивания
        /// </summary>
        private Command downloadCommand;

        /// <summary>
        /// инициализация мьюмодели
        /// </summary>
        public MainVM()
        {
            Dispatcher = new ProgressDispatcher();
        }

        /// <summary>
        /// диспетчер для работы со статусом прогресса
        /// </summary>
        public ProgressDispatcher Dispatcher { get; set; }

        public Command DownLoadCommand
        {
            get
            {
                return downloadCommand ?? (downloadCommand = new Command(
                    async obj =>
                    {
                        var resultWork = await Work();
                        if (resultWork == 100)
                        {
                            Dispatcher.StatusName = "Обучение завершено";
                            Dispatcher.ProgressValue = 100;
                        }
                        else
                        {
                            Dispatcher.StatusName = "Обучение отменено";
                            Dispatcher.ProgressValue = 0;
                        }
                    }));
            }
        }

        public Command СancelCommand
        {
            get
            {
                return cancelCommand ?? (cancelCommand = new Command(
                    obj => cancelTokenSource.Cancel()));
            }
        }

        /// <summary>
        /// метод скачивания телеграмма
        /// </summary>
        /// <returns></returns>
        private async Task DownLoad()
        {
            cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            await Task.Run(
                () =>
                {
                    var url = new Uri(@"https://telegram.org/dl/desktop/win64");
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += (s, ee) =>
                        {
                            if (token.IsCancellationRequested)
                            {
                                client.CancelAsync();
                                Dispatcher.StatusName = "Операция отменена";
                                Dispatcher.ProgressValue = 0;
                                return;
                            }

                            var sizeDownloaded = (ee.BytesReceived / 1048576).ToString("#.# МБ");

                            Dispatcher.StatusName = $"Идет загрузка: {ee.ProgressPercentage.ToString()}% ({sizeDownloaded})";
                            Dispatcher.ProgressValue = ee.ProgressPercentage;

                            if (Dispatcher.ProgressValue == 100)
                                Dispatcher.StatusName = "Загрузка завершена\nПроверьте рабочий стол";
                        };

                        client.DownloadFileAsync(
                            url,
                            $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\Установщик телеги.exe");
                    }
                });
        }

        /// <summary>
        /// Метод имитирующий работу.
        /// </summary>
        /// <returns></returns>
        private async Task<int> Work()
        {
            cancelTokenSource = new CancellationTokenSource();
            var token = cancelTokenSource.Token;

            return await Task.Run(
                () =>
                {
                    for (var i = 0; i < 100; i++)
                    {
                        if (token.IsCancellationRequested)
                            return 0;

                        Dispatcher.StatusName = $"Обучение искуственного интеллекта {i}%";
                        Dispatcher.ProgressValue++;
                        Thread.Sleep(50);
                    }

                    return 100;
                });
        }
    }
}