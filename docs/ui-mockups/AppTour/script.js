const states = [
    {
        id: "menu",
        step: 1,
        title: "Start from the help menu",
        copy: "Open the <strong>?</strong> help menu any time you want a quick walkthrough of the app.",
        highlight: ["target-help", "target-menu"],
        showMenu: true,
        showIntro: false,
        nextLabel: "Next"
    },
    {
        id: "intro",
        step: 1,
        title: "Introduce the tour clearly",
        copy: "An optional intro dialog sets expectations before the walkthrough begins and keeps the flow informational rather than task-driven.",
        highlight: ["target-intro"],
        showMenu: false,
        showIntro: true,
        nextLabel: "Next"
    },
    {
        id: "drawer",
        step: 2,
        title: "Show how to find calculators",
        copy: "The tour can spotlight the navigation drawer to explain how calculators are grouped and how users move around the app.",
        highlight: ["target-drawer"],
        showMenu: false,
        showIntro: false,
        nextLabel: "Next"
    },
    {
        id: "inputs",
        step: 3,
        title: "Explain the input area",
        copy: "Highlight the calculator inputs to explain that fields validate immediately and update results without a separate submit button.",
        highlight: ["target-inputs"],
        showMenu: false,
        showIntro: false,
        nextLabel: "Next"
    },
    {
        id: "results",
        step: 4,
        title: "Summarize charts and outputs",
        copy: "Use a focused callout for the results panel so users understand where to read the chart, summary values, and growth content.",
        highlight: ["target-results", "target-growth"],
        showMenu: false,
        showIntro: false,
        nextLabel: "Done"
    },
    {
        id: "done",
        step: 4,
        title: "Restart any time from Help",
        copy: "Completing the tour can be remembered locally, but the <strong>Take a tour</strong> menu item should always stay available for quick re-entry.",
        highlight: ["target-help"],
        showMenu: false,
        showIntro: false,
        nextLabel: "Done"
    }
];

const scenarioButtons = Array.from(document.querySelectorAll(".scenario-button"));
const prevStepButton = document.getElementById("prevStep");
const nextStepButton = document.getElementById("nextStep");
const stepNumber = document.getElementById("stepNumber");
const stepTitle = document.getElementById("stepTitle");
const stepCopy = document.getElementById("stepCopy");
const helpMenu = document.querySelector(".help-menu");
const introDialog = document.querySelector(".intro-dialog");
const highlightTargets = Array.from(document.querySelectorAll(".target"));

let currentIndex = 0;

function setStateByIndex(index) {
    currentIndex = Math.max(0, Math.min(index, states.length - 1));
    const state = states[currentIndex];

    scenarioButtons.forEach(button => {
        const isActive = button.dataset.state === state.id;
        button.classList.toggle("is-active", isActive);
        button.setAttribute("aria-selected", String(isActive));
    });

    highlightTargets.forEach(target => {
        const matches = state.highlight.some(className => target.classList.contains(className));
        target.classList.toggle("is-highlighted", matches);
        target.classList.toggle("is-dimmed", !matches && target !== introDialog && target !== helpMenu);
    });

    helpMenu.classList.toggle("is-hidden", !state.showMenu);
    introDialog.classList.toggle("is-hidden", !state.showIntro);

    if (state.showMenu) {
        helpMenu.classList.add("is-highlighted");
    } else {
        helpMenu.classList.remove("is-highlighted");
    }

    if (state.showIntro) {
        introDialog.classList.add("is-highlighted");
    } else {
        introDialog.classList.remove("is-highlighted");
    }

    stepNumber.textContent = String(state.step);
    stepTitle.textContent = state.title;
    stepCopy.innerHTML = state.copy;
    prevStepButton.disabled = currentIndex === 0;
    nextStepButton.textContent = state.nextLabel;
}

scenarioButtons.forEach((button, index) => {
    button.addEventListener("click", () => setStateByIndex(index));
});

prevStepButton.addEventListener("click", () => setStateByIndex(currentIndex - 1));
nextStepButton.addEventListener("click", () => {
    if (currentIndex === states.length - 1) {
        setStateByIndex(0);
        return;
    }

    setStateByIndex(currentIndex + 1);
});

setStateByIndex(0);
