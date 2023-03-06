
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculationLib;

/// <summary> Класс вычисления строки посредством стека. </summary>
public class StackCalculation
{
    private const string ErrCalc = "err";
    private readonly OperationPriority _priority = new();

    private readonly List<string> _calcList = new();

    private readonly Stack<char> _chars = new();
    private readonly Stack<double> _numbs = new();

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
            StackCalc(true);
            if (_chars.Count == 1) IntermediateCalculation(_chars.Pop());
        }
        catch (DivideByZeroException) { return ErrCalc; }
        catch (OverflowException) { return ErrCalc; }
        catch (Exception) { return ErrCalc; }

        var result = _numbs.Pop().ToString();
        CleanProps();

        return result;
    }

    private void CleanProps()
    {
        _calcList.Clear();
        _chars.Clear();
        _numbs.Clear();
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

        //для визуального упрощения разбираем строку на числа и операторы
        AddNumbsAndOperatorsToStack(calcString);

        bool isNumb;
        for (int i = 0; i < _calcList.Count; i++)
        {
            isNumb = double.TryParse(_calcList[i], out double numb);
            if (isNumb) 
            { 
                _numbs.Push(numb);
                continue;
            }

            //добавление символов операторов и скобок в стек
            AddCharToStack(_calcList[i][0]);
        }
    }

    /// <summary> Добавить все числа и операторы в стек. </summary>
    /// <param name="calcString"> Вычисляемая строка. </param>
    /// <remarks> Метод облегчает работу с циклом по операторам и числам в стеках. </remarks>
    private void AddNumbsAndOperatorsToStack(string calcString)
    {
        StringBuilder sb = new();

        //создаем коллекцию из чисел и операторов
        for (int i = 0; i < calcString.Length; i++)
        {
            //добавление числовых символов в стек
            if (char.IsNumber(calcString[i]) || calcString[i] == ',' || calcString[i] == '.')
            {
                sb.Append(calcString[i]);
                if (i == calcString.Length - 1)
                    _calcList.Add(sb.ToString());

                continue;
            }

            //если число из билдера еще не добавлено в стек, то добавляем
            if (sb.Length > 0)
            {
                _calcList.Add(sb.ToString());
                sb = new();
            }

            _calcList.Add(calcString[i].ToString());
        }
    }

    /// <summary> Добавить символ (операторы, скобки, символ корня) в стек символов. </summary>
    /// <param name="c"> Добавляемый символ. </param>
    private void AddCharToStack(char c)
    {
        _chars.Push(c);
        if (_chars.Count < 2 || _chars.Peek() == '(' || _numbs.Count == 0) return;
        StackCalc();
    }

    /// <summary> Рассчет в стеке на текущий момент. </summary>
    /// <param name="addedAllOperators"> Параметр, указывающий, что все операторы добавлены в стек. </param>
    private void StackCalc(bool addedAllOperators = false)
    {
        if(_chars.Count < 2) return;
        char lastChar = _chars.Pop();
        char prevChar = _chars.Pop();

        if(lastChar == 's')
        {
            _numbs.Push(Math.Sqrt(_numbs.Pop()));
            _chars.Push(prevChar);
            StackCalc();
            return;
        }

        if(!addedAllOperators && (_priority.Priority.GetValueOrDefault(lastChar) > _priority.Priority.GetValueOrDefault(prevChar) ||
            lastChar == '(' ||
            (prevChar == '(' && lastChar != ')')))
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
            if (_chars.Count > 0)
            {
                prevChar = _chars.Pop();
                StackCalc();
                return;
            }
            else
                return;
        }

        if (prevChar == '(' && lastChar == ')')
        {
            //если ничего кроме скобок, то схлопываем
            if (_chars.Count == 0) return;

            ////если следующий оператор - корень
            //if (_chars.Peek() == 's')
            //{
            //    _chars.Pop();
            //    _numbs.Push(Math.Sqrt((double)_numbs.Pop()));
            //}

            StackCalc();
            return;
        }

        //если приоритетность последнего добавленного ниже или равна предпоследнему, то выполняем вычисление с предпоследним
        //за тем возвращаем в стек последний оператор
        while (prevChar != '(' && prevChar != ')' &&
            _priority.Priority.GetValueOrDefault(lastChar) <= _priority.Priority.GetValueOrDefault(prevChar))
        {
            IntermediateCalculation(prevChar);

            if (_chars.Count > 0 && _chars.Peek() == '(')
            {
                _chars.Push(lastChar);
                return;
            }

            _chars.Push(lastChar);

            if (_chars.Count > 0)
            {
                StackCalc();
                return;
            }
        }

        if (addedAllOperators)
        {
            IntermediateCalculation(lastChar);
            _chars.Push(prevChar);
            StackCalc();
        }
    }

    /// <summary> Промежуточное вычисление двух чисел. </summary>
    /// <param name="op"> Оператор. </param>
    /// <returns> Результат вычисления. </returns>
    private void IntermediateCalculation(char op)
    {
        if (op == 's')
        {
            _numbs.Push(Math.Sqrt(_numbs.Pop()));
            return;
        }

        if (_numbs.Count < 2) return;

        //забираем числа и оператор
        var secondNumb = _numbs.Pop();
        var firstNumb = _numbs.Pop();

        

        //производим калькуляцию
        _numbs.Push(NambsCalculate(firstNumb, secondNumb, op));
    }

    /// <summary> Вычисление посредством стека. </summary>
    /// <returns> Результат вычисления. </returns>
    private double NambsCalculate(double firstNumb, double secondNumb, char op)
    {
        Debug.Print($"{firstNumb}  {op}  {secondNumb}");

        var result = op switch
        {
            '+' => firstNumb + secondNumb,
            '-' => firstNumb - secondNumb,
            '*' => firstNumb * secondNumb,
            '/' => firstNumb / secondNumb,
            '^' => Math.Pow(firstNumb, secondNumb),
            _ => 0,
        };

        if (double.IsInfinity(result))
            throw new DivideByZeroException();
        else
            return result;
    }
}