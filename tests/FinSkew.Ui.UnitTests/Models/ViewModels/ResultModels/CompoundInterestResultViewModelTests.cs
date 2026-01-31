//namespace FinSkew.Ui.UnitTests.Models.ViewModels.ResultModels;

//public class CompoundInterestResultViewModelTests
//{
//    [Fact]
//    public void Constructor_ShouldAllowPropertyInitialization()
//    {
//        // Arrange
//        var input = new CompoundInterestInputViewModel
//        {
//            PrincipalAmount = 10000,
//        };

//        // Act
//        var result = new CompoundInterestResultViewModel
//        {
//            Inputs = input,
//            TotalInterestEarned = 1608,
//            TotalAmount = 11608
//        };

//        // Assert
//        result.Should().NotBeNull();
//        result.Inputs.PrincipalAmount.Should().Be(10000);
//        result.TotalInterestEarned.Should().Be(1608);
//        result.TotalAmount.Should().Be(11608);
//    }

//    [Theory]
//    [InlineData(10000, 1608, 11608)]
//    [InlineData(50000, 12166, 62166)]
//    [InlineData(100000, 79084, 179084)]
//    public void Properties_ShouldAcceptValidValues(int principal, int interest, int total)
//    {
//        // Arrange
//        var input = new CompoundInterestInputViewModel
//        {
//            PrincipalAmount = 10000,
//        };

//        // Act
//        var result = new CompoundInterestResultViewModel
//        {
//            Inputs = input,
//            TotalInterestEarned = interest,
//            TotalAmount = total
//        };

//        // Assert
//        result.Inputs.PrincipalAmount.Should().Be(principal);
//        result.TotalInterestEarned.Should().Be(interest);
//        result.TotalAmount.Should().Be(total);
//    }

//    [Fact]
//    public void Properties_ShouldBeSettable()
//    {
//        // Arrange
//        var result = new CompoundInterestResultViewModel
//        {
//            PrincipalAmount = 10000,
//            TotalInterestEarned = 1000,
//            TotalAmount = 11000
//        };

//        // Act
//        result.PrincipalAmount = 20000;
//        result.TotalInterestEarned = 2000;
//        result.TotalAmount = 22000;

//        // Assert
//        result.Inputs.PrincipalAmount.Should().Be(20000);
//        result.TotalInterestEarned.Should().Be(2000);
//        result.TotalAmount.Should().Be(22000);
//    }

//    [Fact]
//    public void ResultViewModel_ConsistencyCheck_TotalShouldEqualPrincipalPlusInterest()
//    {
//        // Arrange
//        var principal = 25000;
//        var interest = 8000;

//        // Act
//        var result = new CompoundInterestResultViewModel
//        {
//            PrincipalAmount = principal,
//            TotalInterestEarned = interest,
//            TotalAmount = principal + interest
//        };

//        // Assert
//        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount + result.TotalInterestEarned);
//    }

//    [Fact]
//    public void ResultViewModel_WithZeroInterest_ShouldHaveTotalEqualToPrincipal()
//    {
//        // Arrange
//        var input = new CompoundInterestInputViewModel
//        {
//            PrincipalAmount = 10000,
//        };

//        // Act
//        var result = new CompoundInterestResultViewModel
//        {
//            Inputs = input,
//            TotalInterestEarned = 0,
//            TotalAmount = input.PrincipalAmount
//        };

//        // Assert
//        result.TotalInterestEarned.Should().Be(0);
//        result.TotalAmount.Should().Be(result.Inputs.PrincipalAmount);
//    }

//    [Fact]
//    public void ResultViewModel_WithBogusData_ShouldAcceptValues()
//    {
//        // Arrange
//        var faker = new Faker();
//        var principal = faker.Random.Int(10000, 100000);
//        var interest = faker.Random.Int(1000, 50000);

//        var input = new CompoundInterestInputViewModel
//        {
//            PrincipalAmount = principal,
//        };

//        // Act
//        var result = new CompoundInterestResultViewModel
//        {
//            Inputs = input,
//            TotalInterestEarned = interest,
//            TotalAmount = principal + interest
//        };

//        // Assert
//        result.Should().NotBeNull();
//        result.Inputs.PrincipalAmount.Should().BeGreaterThan(0);
//        result.TotalInterestEarned.Should().BeGreaterThan(0);
//        result.TotalAmount.Should().Be(principal + interest);
//    }

//    [Fact]
//    public void ResultViewModel_DefaultValues_ShouldBeZero()
//    {
//        // Act
//        var result = new CompoundInterestResultViewModel();

//        // Assert
//        result.Inputs.PrincipalAmount.Should().Be(0);
//        result.TotalInterestEarned.Should().Be(0);
//        result.TotalAmount.Should().Be(0);
//    }

//    [Theory]
//    [InlineData(10000, 1608, 11608)]
//    [InlineData(50000, 24328, 74328)]
//    public void ResultViewModel_MultipleInstances_ShouldBeIndependent(int principal1, int interest1, int total1)
//    {
//        // Arrange & Act
//        var result1 = new CompoundInterestResultViewModel
//        {
//            PrincipalAmount = principal1,
//            TotalInterestEarned = interest1,
//            TotalAmount = total1
//        };

//        var result2 = new CompoundInterestResultViewModel
//        {
//            PrincipalAmount = 20000,
//            TotalInterestEarned = 3000,
//            TotalAmount = 23000
//        };

//        // Assert
//        result1.Inputs.PrincipalAmount.Should().Be(principal1);
//        result1.TotalInterestEarned.Should().Be(interest1);
//        result2.Inputs.PrincipalAmount.Should().Be(20000);
//        result2.TotalInterestEarned.Should().Be(3000);
//    }
//}

