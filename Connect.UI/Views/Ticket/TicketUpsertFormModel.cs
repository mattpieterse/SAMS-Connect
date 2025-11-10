using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.DependencyInjection;
using Connect.Data.Models;
using Connect.UI.Models;
using Connect.UI.Models.Data;
using FluentValidation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using static System.Text.RegularExpressions.Regex;

namespace Connect.UI.Views.Ticket;

public sealed partial class TicketUpsertFormModel
    : ReactiveForm<TicketDto>
{
#region Variables

    [Reactive]
    private string _heading = string.Empty;


    [Reactive]
    private string _content = string.Empty;


    [Reactive]
    private MunicipalDepartment? _category;


    [Reactive]
    private ObservableCollection<FileAttachment> _attachments = [];


    [ObservableAsProperty]
    private IReadOnlyList<TypedComboBoxOption<MunicipalDepartment>> _categoryOptions = [];

#endregion

#region Lifecycle

    /// <summary>
    /// Constructor for <see cref="TicketUpsertFormModel"/>
    /// </summary>
    public TicketUpsertFormModel() {
        _categoryOptionsHelper = Observable
            .Return(Enum.GetValues(typeof(MunicipalDepartment))
                .Cast<MunicipalDepartment>()
                .Select(e => new TypedComboBoxOption<MunicipalDepartment>(
                    e,
                    CamelCaseMatch().Replace(e.ToString(), " $1")
                ))
                .ToList()
                .AsReadOnly()
            )
            .ToProperty(this, p => p.CategoryOptions);

        InitializeReactivity();
    }


    public void Clear() {
        _heading = string.Empty;
        _content = string.Empty;
        _category = null;
        _attachments = [];
    }

#endregion

#region Internals

    protected override IValidator<TicketDto> Validator =>
        Ioc.Default.GetRequiredService<IValidator<TicketDto>>();


    protected override IEnumerable<IObservable<object?>> ValidationTriggers
    {
        get {
            yield return this.WhenAnyValue(p => p.Category).Select(o => (object?) o);
            yield return this.WhenAnyValue(p => p.Heading);
            yield return this.WhenAnyValue(p => p.Content);
        }
    }


    protected override IEnumerable<IObservable<bool>> CompletionTriggers
    {
        get {
            yield return this.WhenAnyValue(p => p.Category).Select(p => p is not null);
            yield return this.WhenAnyValue(p => p.Heading).Select(p => !string.IsNullOrWhiteSpace(p));
            yield return this.WhenAnyValue(p => p.Content).Select(p => !string.IsNullOrWhiteSpace(p));
        }
    }


    protected override void SynchronizeEntity() {
        Entity.Category = Category;
        Entity.Heading = Heading;
        Entity.Content = Content;
    }

#endregion

#region Regex

    [GeneratedRegex("(\\B[A-Z])")]
    private static partial Regex CamelCaseMatch();

#endregion
}
