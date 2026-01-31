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
