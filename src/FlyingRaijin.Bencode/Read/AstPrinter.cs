using FlyingRaijin.Bencode.Read.Ast.Base;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingRaijin.Bencode.Read
{
    public static class AstPrinter
    {
        public static string Print(NodeBase ast)
        {
            int level = 0;

            var sb = new StringBuilder();

            var stack = new Stack<NodeBase>();

            stack.Push(ast);

            while (stack.Count > 0)
            {
                NodeBase current = stack.Pop();

                if (current is SentinelNode)
                {
                    level--;
                    continue;
                }

                sb.Append('\t', level);

                sb.Append(current.ProductionType.ToString());

                if (current != null && current.Children.Count > 0)
                {
                    stack.Push(new SentinelNode());

                    level++;

                    for (int i = (current.Children.Count-1); i >= 0; i--)
                    {
                        stack.Push(current.Children.ElementAt(i));
                    }                    
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public static string _Print(NodeBase ast)
        {
            int level = 0;

            var sb = new StringBuilder();

            PrintRecursive(ast, sb, ref level);

            return sb.ToString();
        }

        public static void PrintRecursive(NodeBase ast, StringBuilder sb, ref int level)
        {
            sb.Append('\t', level);
            sb.Append(ast.ProductionType.ToString());
            sb.AppendLine();

            if (ast.Children != null && ast.Children.Count > 0)
            {
                level++;

                foreach (var item in ast.Children)
                {
                    PrintRecursive(item, sb, ref level);
                }

                level--;
            }
        }
    }
}