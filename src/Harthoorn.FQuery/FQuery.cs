using Harthoorn.Dixit;

namespace Harthoorn.FQuery
{ 
    public static class FQL
    {
        public static Language Language;

        public static ISyntax
           keyword, identifier;

        public static IGrammar
            Whitespace, Wildcard, FieldName, Comma, StringValue,
            Field, EqualityOp, FieldList, FromClause, EqualityExpression,
            BooleanOp, BooleanExpression, BracketsExpression, Expression,
            WhereClause, Statement, OptionalWhereClause;

        static FQL()
        {
            Language = new Language();

            Whitespace = Language.WhiteSpace(' ', '\n', '\t');
            keyword = new CharSet(2, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", "abcdefghijklmnopqrstuvwxyz");
            identifier = new Identifier();

            Wildcard = Language.Literal("Wildcard", "*");
            FieldName = Language.Syntax("FieldName", identifier);

            Comma = Language.Literal("Comma", ",");
            Field = Language.Any("Field");

            StringValue = Language.Delimit("string", '\'', '\'', '\\');
            EqualityOp = Language.Any("EqualityOperator");
            FieldList = Language.Interlace("FieldList", ",");
            FromClause = Language.Sequence("FromClause");
            EqualityExpression = Language.Sequence("Equation");
            BooleanOp = Language.Any("BooleanOperator");
            BooleanExpression = Language.Sequence("BooleanExpression");
            BracketsExpression = Language.Sequence("BracketExpressions");
            Expression = Language.Any("Expression");
            WhereClause = Language.Sequence("WhereClause");
            Statement = Language.Sequence("SelectStatement");

            OptionalWhereClause = Language.Optional("OptionalWhereClause");

            Field
                .Define(FieldName, Wildcard);

            EqualityOp
                .Define("=", "!=", "<", ">");

            FieldList
                .Define(Field);

            FromClause
                .Define(FieldName);

            EqualityExpression
                .Define(FieldName, EqualityOp, StringValue);

            BooleanOp
                .Define("and", "or");

            BooleanExpression
                .Define(EqualityExpression, BooleanOp, Expression);

            BracketsExpression
                .Define("(", Expression, ")");

            Expression
                .Define(BooleanExpression, EqualityExpression, BracketsExpression);

            WhereClause
                .Define("where", Expression);

            OptionalWhereClause
                .Define(WhereClause);

            Statement
                .Define("select", FieldList, "from", FromClause, OptionalWhereClause);

            Language.Root(Statement);
        }
    }

}