using System;

public class NodeEventArgs : EventArgs
{
    public readonly Node Node;
    public NodeEventArgs(Node node)
    {
        this.Node = node;
    }
}