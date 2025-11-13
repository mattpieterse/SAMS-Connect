namespace Connect.Data.Caches.Algorithms;

public class MaxHeap<T>(Comparison<T> comparer)
{
    private readonly List<T> _heap = [];
    private readonly Comparison<T> _comparer = comparer;

    public int Count => _heap.Count;

    public void Insert(T item) {
        _heap.Add(item);
        HeapifyUp(
            _heap.Count - 1
        );
    }

    public T ExtractMax() {
        if (_heap.Count == 0)
            throw new InvalidOperationException();

        var max = _heap[0];
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        HeapifyDn(0);
        return max;
    }


    private void HeapifyUp(int index) {
        while (index > 0) {
            var parentIndex = ((index - 1) / 2);
            if (_comparer(_heap[index], _heap[parentIndex]) <= 0) {
                break;
            }

            // Swap the values
            (
                _heap[parentIndex],
                _heap[index]
            ) = (
                _heap[index],
                _heap[parentIndex]
            );

            index = parentIndex;
        }
    }


    private void HeapifyDn(int index) {
        var previous = (_heap.Count - 1);
        while (true) {
            var lSide = (2 * index) + 1;
            var rSide = (2 * index) + 2;
            var largest = index;

            if (lSide <= previous && _comparer(_heap[lSide], _heap[largest]) > 0)
                largest = lSide;

            if (rSide <= previous && _comparer(_heap[rSide], _heap[largest]) > 0)
                largest = rSide;

            if (largest == index)
                break;

            // Swap the values
            (
                _heap[index],
                _heap[largest]
            ) = (
                _heap[largest],
                _heap[index]
            );

            index = largest;
        }
    }
}
