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
    const growthChartBars = document.getElementById('growthChartBars');
    const growthTableBody = document.getElementById('growthTableBody');

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

        renderGrowth(M, r, N);
    }

    function renderGrowth(monthlyInvestment, monthlyRate, yearsInput) {
        const years = Math.max(1, Math.round(yearsInput));
        const growthData = [];

        for (let year = 1; year <= years; year++) {
            const months = year * 12;
            const total = monthlyRate === 0
                ? monthlyInvestment * months
                : monthlyInvestment * ((Math.pow(1 + monthlyRate, months) - 1) / monthlyRate) * (1 + monthlyRate);

            growthData.push({ year, total });
        }

        const maxValue = growthData.reduce((max, item) => Math.max(max, item.total), 0);
        growthChartBars.innerHTML = '';
        growthTableBody.innerHTML = '';

        growthData.forEach(item => {
            const barHeight = maxValue > 0 ? (item.total / maxValue) * 100 : 0;

            const barItem = document.createElement('div');
            barItem.className = 'growth-bar-item';
            barItem.innerHTML = `
                <div class="growth-bar-track" title="Year ${item.year}: ${formatCurrency(item.total)}">
                    <div class="growth-bar-fill" style="height:${barHeight}%;"></div>
                </div>
                <div class="growth-bar-year">Y${item.year}</div>
            `;
            growthChartBars.appendChild(barItem);

            const row = document.createElement('tr');
            row.innerHTML = `
                <td>Year ${item.year}</td>
                <td>${formatCurrency(item.total)}</td>
            `;
            growthTableBody.appendChild(row);
        });
    }

    // Attach listeners
    [monthlyInvestmentInput, returnRateInput, timePeriodInput].forEach(input => {
        input.addEventListener('input', calculate);
    });

    // Initial calculation
    calculate();
});
