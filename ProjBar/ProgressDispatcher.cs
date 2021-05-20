namespace ProjBar
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using ProjBar.Annotations;

    /// <summary>
    /// Управление выводом
    /// </summary>
    public class ProgressDispatcher : INotifyPropertyChanged
    {
        private double _progressValue;

        private string _statusName;

        /// <summary>
        /// общее значение прогресса
        /// </summary>
        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged(nameof(ProgressValue));
            }
        }

        /// <summary>
        /// заголовок выполняемой операции
        /// </summary>
        public string StatusName
        {
            get => _statusName;
            set
            {
                _statusName = value;
                OnPropertyChanged(nameof(StatusName));
            }
        }

        /// <summary>
        /// штука чтобы отслеживать изменения
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName]
            string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}