using System;
using System.Collections.Generic;

namespace Clones
{
    public class CloneVersionSystem : ICloneVersionSystem
    {
        private List<Clone> clones;

        public CloneVersionSystem()
        {
            clones = new List<Clone> {new Clone()};
        }

        public string Execute(string query)
        {
            var str = query.Split(' ');
            var number = int.Parse(str[1]) - 1;
            return Commands(str, number);
        }

        public string Commands(string[] query, int number)
        {
            switch (query[0])
            {
                case "learn":
                    clones[number].Learn(query[2]);
                    break;
                case "rollback":
                    clones[number].Rollback();
                    break;
                case "relearn":
                    clones[number].Relearn();
                    break;
                case "clone":
                    clones.Add(clones[number].CloneThis());
                    break;
                case "check":
                    return clones[number].Check();
            }

            return null;
        }

        public class Clone
        {
            private Stack<string> learningProgram;
            private Stack<string> rollbackProgram;

            public Clone()
            {
                learningProgram = new Stack<string>();
                rollbackProgram = new Stack<string>();
            }

            public Clone(Stack<string> learningProgram, Stack<string> rollbackProgram)
            {
                this.learningProgram = learningProgram;
                this.rollbackProgram = rollbackProgram;
            }

            public void Learn(string program)
            {
                learningProgram.AddLast(program);
            }

            public void Rollback()
            {
                rollbackProgram.AddLast(learningProgram.Tail.Value);
                learningProgram.RemoveLast();
            }

            public void Relearn()
            {
                learningProgram.AddLast(rollbackProgram.Tail.Value);
                rollbackProgram.RemoveLast();
            }

            public Clone CloneThis()
            {
                return new Clone(new Stack<string>
                    {
                        Head = learningProgram.Head, Tail = learningProgram.Tail, Count = learningProgram.Count
                    },
                    new Stack<string>
                    {
                        Head = rollbackProgram.Head, Tail = rollbackProgram.Tail, Count = rollbackProgram.Count
                    });
            }

            public string Check()
            {
                return learningProgram.Count > 0 ? learningProgram.Tail.Value : "basic";
            }
        }
        
        public class StackItem<T>
        { 
            public T Value { get; set; }
            public StackItem<T> Previous { get; set; }
        }

        public class Stack<T>
        {
            public StackItem<T> Head { get; set; }
            public StackItem<T> Tail{ get; set; }
            public int Count { get; set; }

            public bool IsEmpty { get { return Head == null; } }

            public void AddLast(T value)
            {
                if (IsEmpty)
                {
                    Tail = Head = new StackItem<T> { Value = value, Previous = null };
                    Count = 1;
                }
                else
                {
                    var item = new StackItem<T> { Value = value, Previous = Tail };
                    Tail = item;
                    Count++;
                }
            }

            public void RemoveLast()
            {
                if (Head == null) throw new InvalidOperationException();
                Tail = Tail.Previous;
                Count--;
            }
        }
    }
}