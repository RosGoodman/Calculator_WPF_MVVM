using CalculationLib;

namespace StackCalculationTests;

public class StackCalculationsTest
{
    private readonly StackCalculation calculation = new ();

    [Fact]
    public void SimpleAdditionInt_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5+5";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("10", result);
    }

    [Fact]
    public void MultipleAdditionInt_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5+5+7+0";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("17", result);
    }

    [Fact]
    public void MultipleAdditionIntWithStaples_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5+5+(7+0)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("17", result);
    }

    [Fact]
    public void MultipleAdditionIntWithMultipleStaples_ItShouldBeEqual()
    {
        //arrange
        string calcString = "(5+5)+((7+0))";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("17", result);
    }

    [Fact]
    public void MultipleSubtractionIntWithMultipleStaples_ItShouldBeEqual()
    {
        //arrange
        string calcString = "(5-5)-((7-0))";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("-7", result);
    }

    [Fact]
    public void SimpleSubtractionInt_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5-5";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("0", result);
    }

    [Fact]
    public void SimpleSubtractionDecimal_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5-4.5";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("0,5", result);
    }

    [Fact]
    public void MultipleSubtractionDecimalWithStaples_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5-4.5-(34-23)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("-10,5", result);
    }

    [Fact]
    public void SimpleSqrt_ItShouldBeEqual()
    {
        //arrange
        string calcString = "sqrt(25)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("5", result);
    }

    [Fact]
    public void SqrtWithAdditions_ItShouldBeEqual()
    {
        //arrange
        string calcString = "sqrt(20+5)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("5", result);
    }

    [Fact]
    public void DifficultSqr_ItShouldBeEqual()
    {
        //arrange
        string calcString = "sqrt(sqrt(500+125))";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("5", result);
    }

    [Fact]
    public void SimpleDivide_ItShouldBeEqual()
    {
        //arrange
        string calcString = "12/3";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("4", result);
    }

    [Fact]
    public void SimpleMultiple_ItShouldBeEqual()
    {
        //arrange
        string calcString = "12*3";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("36", result);
    }

    [Fact]
    public void SimpleMultipleWithDivide_ItShouldBeEqual()
    {
        //arrange
        string calcString = "12*3/6";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("6", result);
    }

    [Fact]
    public void DifficuleExample_ItShouldBeEqual()
    {
        //arrange
        string calcString = "1+2*(3+4/2-(1+2))*2+1";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("10", result);
    }

    [Fact]
    public void DivideByZero_ItShouldBeErrMessage()
    {
        //arrange
        string calcString = "4/0";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("err", result);
    }

    [Fact]
    public void BigNumbMultiple_ItShouldBeErrMessage()
    {
        //arrange
        string calcString = 
            "432452345234567234523234523452345234523452435325234333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333" +
            "*" +
            "234523453505467572345234532452345234523234523523454233333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333333";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("err", result);
    }

    [Fact]
    public void MixingComputing_ItShouldBeEqual()
    {
        //arrange
        string calcString = "5-3*(8)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("-19", result);
    }

    [Fact]
    public void SimpleSqrt_2_ItShouldBeEqual()
    {
        //arrange
        string calcString = "sqrt(9)";

        //act
        var result = calculation.Calculate(calcString);

        //assert
        Assert.Equal("3", result);
    }
}