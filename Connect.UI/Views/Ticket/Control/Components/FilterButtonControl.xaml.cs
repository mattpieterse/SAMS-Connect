using System.Reactive.Disposables.Fluent;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace Connect.UI.Views.Ticket.Control.Components;

public partial class FilterButtonControl
{
#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="FilterButtonControl"/>
    /// </summary>
    public FilterButtonControl() {
        InitializeComponent();
        this.WhenActivated(disposables => {
            this.OneWayBind(
                ViewModel,
                bind => bind.ListFilters.DepartmentOptions,
                view => view.FilterDepartments.ItemsSource
            ).DisposeWith(disposables);

            this.Bind(
                ViewModel,
                bind => bind.ListFilters.FilterStartDate,
                view => view.FilterDateStartSelector.SelectedDate
            ).DisposeWith(disposables);

            this.Bind(
                ViewModel,
                bind => bind.ListFilters.FilterFinalDate,
                view => view.FilterDateFinalSelector.SelectedDate
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.AppliedFiltersCount,
                view => view.FilterInfoBadge.Value
            ).DisposeWith(disposables);

            this.OneWayBind(
                ViewModel,
                bind => bind.AppliedFiltersCount,
                view => view.FilterInfoBadge.Visibility,
                UiConverters.IntToVisibility
            ).DisposeWith(disposables);

            this.BindCommand(
                ViewModel,
                bind => bind.ClearFiltersCommand,
                view => view.ClearFiltersButton
            ).DisposeWith(disposables);
        });
    }

#endregion

#region Internals

    private static class UiConverters
    {
        public static Visibility IntToVisibility(int value) =>
            (value == 0) ? Visibility.Collapsed : Visibility.Visible;
    }

#endregion
}
