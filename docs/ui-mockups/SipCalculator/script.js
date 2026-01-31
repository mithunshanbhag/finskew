document.addEventListener('DOMContentLoaded', () => {
    // Inputs
    const monthlyInvestmentInput = document.getElementById('monthlyInvestment');
    const returnRateInput = document.getElementById('returnRate');
    const timePeriodInput = document.getElementById('timePeriod');

    // Outputs
    const investedAmountResult = document.getElementById('investedAmountResult');
    const estReturnsResult = document.getElementById('estReturnsResult');
    const totalValueResult = document.getElementById('totalValueResult');
    const totalValueHero = document.getElementById('totalValueHero');
    const resultChart = document.getElementById('resultChart');

    // Format currency
    const formatCurrency = (value) => {
        return new Intl.NumberFormat('en-IN', {
            style: 'currency',
            currency: 'INR',
            maximumFractionDigits: 0
        }).format(value);
    };

    // Calculate SIP
    function calculate() {
        const M = parseFloat(monthlyInvestmentInput.value) || 0;
        const R = parseFloat(returnRateInput.value) || 0;
        const N = parseFloat(timePeriodInput.value) || 0;

        if (M <= 0 || R <= 0 || N <= 0) return;

        // Formula: A = M × [((1 + r)^n - 1) / r] × (1 + r)
        // r = R / (12 * 100)
        // n = N * 12

        const r = R / (12 * 100);
        const n = N * 12;

        const totalValue = M * ( (Math.pow(1 + r, n) - 1) / r ) * (1 + r);
        const investedAmount = M * n;
        const estReturns = totalValue - investedAmount;

        // Update UI
        investedAmountResult.textContent = formatCurrency(investedAmount);
        estReturnsResult.textContent = formatCurrency(estReturns);
        totalValueResult.textContent = formatCurrency(totalValue);
        totalValueHero.textContent = formatCurrency(totalValue);

        // Update Chart
        const total = totalValue;
        const investedPercentage = (investedAmount / total) * 100;

        // Using brand colors: 
        // Invested (Principal): Brand Blue (#1E88E5)
        // Returns (Interest): Brand Green (#43A047)
        
        const colorInvested = '#1E88E5'; 
        const colorReturns = '#43A047'; 

        // Conic gradient: Start with Invested (Blue), then Returns (Green)
        resultChart.style.background = `conic-gradient(
            ${colorInvested} 0% ${investedPercentage}%, 
            ${colorReturns} ${investedPercentage}% 100%
        )`;
    }

    // Attach listeners
    [monthlyInvestmentInput, returnRateInput, timePeriodInput].forEach(input => {
        input.addEventListener('input', calculate);
    });

    // Initial calculation
    calculate();
});
