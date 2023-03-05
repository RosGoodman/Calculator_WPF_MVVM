
using System.Text;
using System.Text.RegularExpressions;

namespace CalculationLib;

/// <summary> Класс вычисления строки посредством стека. </summary>
public class StackCalculation
{
    private const string ErrCalc = "err";
    private readonly OperationPriority _priority = new();

    private readonly Stack<char> _chars = new();
    private readonly Stack<char> _sqrtChars = new();
    private readonly Stack<decimal> _numbs = new();

    /// <summary> Вычислить выражение в строке. </summary>
    /// <param name="calcString"> Вычисляемая строка. </param>
    /// <returns> Результат вычисления. </returns>
    /// <remarks> "err" - ошибка вычисления. </remarks>
    public string Calculate(string calcString)
    {
        if (!ValidationInputString(calcString)) return ErrCalc;

        try
        {
            AddNumbsAndSymbolsToStack(calcString);
            StackCalc();
        }
        catch (DivideByZeroException) { return ErrCalc; }
        catch (OverflowException) { return ErrCalc; }
        catch (Exception) { return ErrCalc; }

        return _numbs.Pop().ToString();
    }

    /// <summary> Проверка стркоки на наличие лишних символов. </summary>
    /// <param name="calcString"> Проверяемая строка. </param>
    /// <returns> Результат проверки. </returns>
    /// <remarks> false - строка не валидна. </remarks>
    private bool ValidationInputString(string calcString)
        => !Regex.IsMatch(
            calcString.Replace("sqrt", ""),
            @"^[a-zA-Z]+$");

    /// <summary> Добавление чисел и символов в стеки. </summary>
    /// <param name="calcString"> Обрабатываемая строка. </param>
    private void AddNumbsAndSymbolsToStack(string calcString)
    {
        //замена для упрощения дальнейшей обработки
        calcString = calcString.Replace("sqrt", "s");
        calcString = calcString.Replace(".", ",");
        calcString = calcString.Replace(" ", "");

        StringBuilder sb = new ();

        for (int i = 0; i < calcString.Length; i++)
        {
            //добавление числовых символов в стек
            if (char.IsNumber(calcString[i]) || calcString[i] == ',' || calcString[i] == '.')
            {
                sb.Append(calcString[i]);
                if(i == calcString.Length - 1)
                {
                    TryAddNumbInStack(sb.ToString());
                    StackCalc();
                }

                continue;
            }

            //если число из билдера еще не добавлено в стек, то добавляем
            if (sb.Length > 0)
            {
                TryAddNumbInStack(sb.ToString());
                sb = new ();
            }

            //добавлений символов корня в стек
            if (calcString[i] == 's')
                AddSqrtCharToStack(calcString[i]);

            //добавление символов операторов и скобок в стек
            AddCharToStack(calcString[i]);
        }
    }

    /// <summary> Добавить символ (операторы, скобки, символ корня) в стек символов. </summary>
    /// <param name="c"> Добавляемый символ. </param>
    private void AddCharToStack(char c)
    {
        _chars.Push(c);
        OperatorAddedToStack();
    }

    /// <summary> Попытаться добавить конвертируемое число в стек. </summary>
    /// <param name="str"> Строка, которая будет преобразована и добавлена в стек чисел. </param>
    private void TryAddNumbInStack(string str)
    {
        bool isNumb = decimal.TryParse(str, out decimal numb);
        if (isNumb) _numbs.Push(numb);
    }

    /// <summary> Добавить символ корня в стек. </summary>
    /// <param name="c"> Добавляемый символ. </param>
    private void AddSqrtCharToStack(char c)
        => _sqrtChars.Append(c);

    /// <summary> Действия после добавления нового оператора в стек. </summary>
    private void OperatorAddedToStack()
    {
        if (_chars.Count < 2 || _chars.Peek() == '(') return;
        StackCalc();
    }

    /// <summary> Вычисление в текущем состоянии стрека. </summary>
    private void StackCalc()
    {
        //проверка приоритетов последних двух операторов
        if(_chars.Count == 0) return;
        char lastChar = _chars.Pop();

        //если оператор последний, то вычисляем без условий
        if(_chars.Count == 0)
        {
            IntermediateCalculation(lastChar);
            return;
        }

        var prevChar = _chars.Pop();

        //если приоритет последнего добавленного оператора выше, то ничего не считаем.
        //также, если символ открывающейся скобки
        if (_priority.Priority.GetValueOrDefault(lastChar) > _priority.Priority.GetValueOrDefault(prevChar) ||
            prevChar == '(' && lastChar != ')' ||
            lastChar == '(' && prevChar == 's')
        {
            //возвращаем все на место и выходим
            _chars.Push(prevChar);
            _chars.Push(lastChar);
            return;
        }

        //вычисление при закрывающейся скобке считаем все внутри, пока не попадется открывающая скобка
        while (lastChar == ')' && prevChar != '(')
        {
            IntermediateCalculation(prevChar);
            if (_chars.Count > 0) prevChar = _chars.Pop();

            if(prevChar == '(' && lastChar == ')')
            {
                _chars.Push(prevChar);
                _chars.Push(lastChar);
                StackCalc();
            }
        }

        //схлопывание скобок с проверкой наличия корня
        while (lastChar == ')' && prevChar == '(')
        {
            if (_chars.Count == 0) break;

            //если следующий оператор - корень
            if (_chars.Peek() == 's')
            {
                _chars.Pop();
                _numbs.Push((decimal)Math.Sqrt((double)_numbs.Pop()));
            }

            StackCalc();
            return;
        }

        //если приоритетность последнего добавленного ниже или равна предпоследнему, то выполняем вычисление с предпоследним
        //за тем возвращаем в стек последний оператор
        while (prevChar != '(' && prevChar != ')' && 
            _priority.Priority.GetValueOrDefault(lastChar) <= _priority.Priority.GetValueOrDefault(prevChar))
        {
            IntermediateCalculation(prevChar);

            if (_chars.Count != 0 && _chars.Peek() == '(')
            {
                _chars.Push(lastChar);
                return;
            }

            if (_chars.Count > 0)
            {
                prevChar = _chars.Pop();
                if (_chars.Count == 0)
                {
                    _chars.Push(prevChar);
                    break;
                }
            }  

            if (_chars.Count == 0) break;
        }

        _chars.Push(lastChar);
    }

    /// <summary> Промежуточное вычисление двух чисел. </summary>
    /// <param name="op"> Оператор. </param>
    /// <returns> Результат вычисления. </returns>
    private void IntermediateCalculation(char op)
    {
        //забираем числа и оператор
        var secondNumb = _numbs.Pop();
        if(_numbs.Count == 0)
        {
            _numbs.Push(secondNumb);
            return;
        }

        var firstNumb = _numbs.Pop();

        //производим калькуляцию
        _numbs.Push(NambsCalculate(firstNumb, secondNumb, op));
    }

    /// <summary> Вычисление посредством стека. </summary>
    /// <returns> Результат вычисления. </returns>
    private decimal NambsCalculate(decimal firstNumb, decimal secondNumb, char op)
    {
        return op switch
        {
            '+' => firstNumb + secondNumb,
            '-' => firstNumb - secondNumb,
            '*' => firstNumb * secondNumb,
            '/' => firstNumb / secondNumb,
            '^' => (decimal)Math.Pow((double)firstNumb, (double)secondNumb),
            _ => 0,
        };
    }
}