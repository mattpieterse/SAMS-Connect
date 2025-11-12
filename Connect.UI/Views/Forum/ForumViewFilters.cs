using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.DependencyInjection;
using Connect.Data.Caches;
using Connect.Data.Models;
using Connect.UI.Models;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Connect.UI.Views.Forum;

public sealed partial class ForumViewFilters
    : ReactiveObject
{
#region Variables

    public ObservableCollection<TypedCheckBoxOption<MunicipalDepartment>> DepartmentOptions { get; }
    public IObservable<IReadOnlyList<TypedCheckBoxOption<MunicipalDepartment>>> SelectedDepartments { get; }


    [Reactive]
    private DateTime? _filterStartDate;


    [Reactive]
    private DateTime? _filterFinalDate;

#endregion

#region Lifecycle

    public ForumViewFilters() {
        DepartmentOptions =
            new ObservableCollection<TypedCheckBoxOption<MunicipalDepartment>>(ConstructDepartmentOptions());

        var observeSelectedDepartments = DepartmentOptions
            .Select(option =>
                option.WhenAnyValue(p => p.IsSelected).Select(_ => Unit.Default)
            );

        SelectedDepartments = observeSelectedDepartments
            .Merge()
            .StartWith(Unit.Default)
            .Select(IReadOnlyList<TypedCheckBoxOption<MunicipalDepartment>> (_) =>
                DepartmentOptions
                    .Where(option => option.IsSelected)
                    .ToList().AsReadOnly()
            );
    }

#endregion

#region Functions

    public void Clear() {
        FilterStartDate = null;
        FilterFinalDate = null;
        foreach (var department in DepartmentOptions) {
            department.IsSelected = false;
        }
    }

#endregion

#region Internals

    private static IEnumerable<TypedCheckBoxOption<MunicipalDepartment>> ConstructDepartmentOptions() {
        var forumCache = Ioc.Default.GetRequiredService<ForumCache>();
        return (
            from MunicipalDepartment department in forumCache.GetAvailableDepartments()
            let displayText = MunicipalDepartmentEnumToDisplayStringConverter().Replace(department.ToString(), " $1")
            select new TypedCheckBoxOption<MunicipalDepartment>(department, displayText)
        );
    }

#endregion

#region Expression

    [
        GeneratedRegex("(\\B[A-Z])")
    ]
    private static partial Regex MunicipalDepartmentEnumToDisplayStringConverter();

#endregion
}
