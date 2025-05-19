using System.Collections.Generic;
using System;
using UnityEngine;
public class PriorityQueue<T>
{
    private List<(T item, float priority)> _heap = new();
    private Dictionary<T, int> _indices = new();


    public int Count => _heap.Count;

    public void Enqueue(T item, float priority)
    {
        _heap.Add((item, priority));
        int index = _heap.Count - 1;
        _indices[item] = index;
        HeapifyUp(index);
    }

    public T Dequeue()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("Priority queue is empty.");

        T minItem = _heap[0].item;
        Swap(0, _heap.Count - 1);

        _heap.RemoveAt(_heap.Count - 1);
        _indices.Remove(minItem);

        HeapifyDown(0);
        return minItem;
    }

    public bool Contains(T item) => _indices.ContainsKey(item);

    public void UpdatePriority(T item, float newPriority)
    {
        if (!_indices.TryGetValue(item, out int index))
            throw new InvalidOperationException("Item not found in the priority queue.");

        float oldPriority = _heap[index].priority;
        _heap[index] = (item, newPriority);

        if (newPriority < oldPriority)
            HeapifyUp(index);
        else
            HeapifyDown(index);
    }

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) / 2;
            if (_heap[i].priority >= _heap[parent].priority) break;
            Swap(i, parent);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        int lastIndex = _heap.Count - 1;
        while (true)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left <= lastIndex && _heap[left].priority < _heap[smallest].priority) smallest = left;
            if (right <= lastIndex && _heap[right].priority < _heap[smallest].priority) smallest = right;
            if (smallest == i) break;

            Swap(i, smallest);
            i = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
        _indices[_heap[i].item] = i;
        _indices[_heap[j].item] = j;
    }
}