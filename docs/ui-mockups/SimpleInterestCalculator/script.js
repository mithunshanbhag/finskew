document.addEventListener('DOMContentLoaded', () => {

    const inputs = {
        principal: document.getElementById('principalAmount'),
        rate: document.getElementById('interestRate'),
        time: document.getElementById('timePeriod')
    };

    const outputs = {
        totalHero: document.getElementById('heroTotalAmount'),
        summaryPrincipal: document.getElementById('summaryPrincipal'),
        summaryInterest: document.getElementById('summaryInterest'),
        summaryTotal: document.getElementById('summaryTotal'),
        growthBars: document.getElementById('growthChartBars'),
        growthTableBody: document.getElementById('growthTableBody'),
        segments: {
            principal: document.querySelector('.donut-segment-principal'),
            interest: document.querySelector('.donut-segment-interest')
        }
    };

    let debounceTimer;

    function formatCurrency(value) {
        return new Intl.NumberFormat('en-IN', {
            style: 'currency',
            currency: 'INR',
            maximumFractionDigits: 0
        }).format(value);
    }

    function calculate() {
        const P = parseFloat(inputs.principal.value) || 0;
        const R = parseFloat(inputs.rate.value) || 0;
        const N = parseFloat(inputs.time.value) || 0;

        // Validation limits (visual feedback could be added here)
        // Calculating regardless for responsiveness

        const interest = (P * R * N) / 100;
        const total = P + interest;

        // Update Text
        outputs.summaryPrincipal.textContent = formatCurrency(P);
        outputs.summaryInterest.textContent = formatCurrency(interest);
        outputs.summaryTotal.textContent = formatCurrency(total);
        outputs.totalHero.textContent = formatCurrency(total);

        // Update Chart
        // Circumference is ~100
        const totalShare = P + interest;
        const pShare = totalShare > 0 ? (P / totalShare) * 100 : 0;
        const iShare = totalShare > 0 ? (interest / totalShare) * 100 : 0;

        // Principal Segment (Blue) starts at top (offset 25)
        // stroke-dasharray: len gap
        outputs.segments.principal.setAttribute('stroke-dasharray', `${pShare} ${100 - pShare}`);
        outputs.segments.principal.setAttribute('stroke-dashoffset', '25');

        // Interest Segment (Green) starts where Principal ends
        // Offset = 25 - pShare. (Because positive offset moves CCW, negative (or huge positive) moves CW? 
        // Actually typical SVG circle: 
        // offset 25 is 12 o'clock. 
        // We want interest to start at (25 - pShare).
        outputs.segments.interest.setAttribute('stroke-dasharray', `${iShare} ${100 - iShare}`);
        outputs.segments.interest.setAttribute('stroke-dashoffset', `${25 - pShare}`);

        renderGrowth(P, R, N);
    }

    function renderGrowth(P, R, N) {
        const years = Math.max(1, Math.round(N));
        const growthData = [];

        for (let year = 1; year <= years; year++) {
            const total = P * (1 + (R / 100) * year);
            growthData.push({ year, total });
        }

        const maxValue = growthData.reduce((max, item) => Math.max(max, item.total), 0);

        outputs.growthBars.innerHTML = '';
        outputs.growthTableBody.innerHTML = '';

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
            outputs.growthBars.appendChild(barItem);

            const row = document.createElement('tr');
            row.innerHTML = `
                <td>Year ${item.year}</td>
                <td>${formatCurrency(item.total)}</td>
            `;
            outputs.growthTableBody.appendChild(row);
        });
    }

    function onInput() {
        clearTimeout(debounceTimer);
        debounceTimer = setTimeout(calculate, 300); // 300ms debounce
    }

    // Attach listeners
    Object.values(inputs).forEach(input => {
        input.addEventListener('input', onInput);
    });

    // Initial calculation
    calculate();

});
