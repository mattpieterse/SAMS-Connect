using System.Reactive.Linq;
using FluentValidation;
using ReactiveUI;
using ReactiveUI.SourceGenerators;

namespace Connect.UI.Models;

public abstract partial class ReactiveForm<T>
    : ReactiveObject where T : class, new()
{
#region Variables

    protected T Entity { get; } = new();


    protected abstract IValidator<T> Validator { get; }


    protected abstract IEnumerable<IObservable<object?>> ValidationTriggers { get; }


    protected abstract IEnumerable<IObservable<bool>> CompletionTriggers { get; }


    [Reactive]
    private Dictionary<string, string?> _errors = new();


    [ObservableAsProperty(
        ReadOnly = false
    )]
    private double _completion;


    [ObservableAsProperty(
        ReadOnly = false
    )]
    private bool _isValid;

#endregion

#region Functions

    protected abstract void SynchronizeEntity();


    protected void InitializeReactivity() {
        var observeErrors = this.WhenAnyValue(p => p.Errors);

        _isValidHelper = observeErrors
            .Select(v => (v.Count == 0))
            .ToProperty(this, p => p.IsValid);

        // Calculate entity completion
        _completionHelper = CompletionTriggers.CombineLatest()
            .DistinctUntilChanged()
            .Select(p => p.Count(c => c) / (double) p.Count)
            .ToProperty(this, p => p.Completion);

        // Throttle validation checker
        ValidationTriggers.Merge()
            .DistinctUntilChanged()
            .Throttle(TimeSpan.FromMilliseconds(200))
            .Subscribe(_ => ValidateForm());
    }

#endregion

#region Internals

    private void ValidateForm() {
        SynchronizeEntity();
        var vr = Validator.Validate(Entity);
        Errors = vr.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                keySelector: g => g.Key,
                elementSelector: string? (g) => g.First().ErrorMessage
            );
    }

#endregion
}
