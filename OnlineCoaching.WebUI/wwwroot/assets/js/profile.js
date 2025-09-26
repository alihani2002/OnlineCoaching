const dayNames = {
    1: "First Day", 2: "Second Day", 3: "Third Day", 4: "Fourth Day",
    5: "Fifth Day", 6: "Sixth Day", 7: "Seventh Day",
    8: "Week 1", 9: "Week 2", 10: "Week 3" /* Adjusted for clearer labeling if these represent weeks */
};

const assignedExercises = window.assignedExercises || [];
const assignedFoods = window.assignedFoods || [];
const clientId = window.clientId || 0;

function groupBy(array, keyFunc) {
    return array.reduce((result, item) => {
        const key = keyFunc(item);
        if (!result[key]) result[key] = [];
        result[key].push(item);
        return result;
    }, {});
}

/* ---------- Exercises ---------- */
function renderExercises() {
    const exercisesByDay = groupBy(assignedExercises, e => e.DayOfWeek);
    const exerciseContainer = document.getElementById('exercise-container');
    exerciseContainer.innerHTML = ''; // Clear existing content

    if (assignedExercises.length === 0) {
        // This case is already handled in the Razor view.
        return;
    }

    for (const [day, exercises] of Object.entries(exercisesByDay)) {
        const dayDiv = document.createElement('div');
        dayDiv.className = 'custom-card mb-4';
        dayDiv.innerHTML = `
            <div class="day-header">
                <span>🏋 ${dayNames[day] ?? `Day ${day}`}</span>
                <span>Exercises</span>
            </div>`;

        const exercisesByMuscle = groupBy(exercises, e => e.MuscleName ?? "Other");
        for (const [muscle, muscleExercises] of Object.entries(exercisesByMuscle)) {
            let muscleBlock = `<div class="muscle-header">💪 ${muscle}</div>`;
            muscleExercises.forEach(e => {
                muscleBlock += `
                    <div class="exercise-card">
                        <div class="exercise-image">
                            ${e.ImageUrl
                        ? `<a href="javascript:void(0);" onclick="openVideo('${e.VideoUrl ?? ''}')">
                                    <img src="${e.ImageUrl}" class="main-img" alt="${e.NameOfExercise}" />
                                </a>`
                        : `<span class="text-muted p-3">No Image</span>`}
                        </div>
                        <div class="exercise-details">
                            <div><strong>${e.NameOfExercise ?? ''}</strong></div>
                            <div>Sets: ${e.Sets ?? 'N/A'}</div>
                            <div>Reps: ${e.Reps ?? 'N/A'}</div>
                            <div>Notes: ${e.Notes || 'None'}</div>
                            <div class="exercise-actions">
                                <a href="/ExerciseSheetLogs/Create?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-warning ">➕ Log</a>
                                <a href="/ExerciseSheetLogs/ClientExerciseLogs?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-info ">📊 Logs</a>
                                <a href="/ExerciseNote/Create?exerciseId=${e.ExerciseId}" class="btn btn-primary">✏️ Add Note</a>
                                <a href="/ExerciseNote/Index?exerciseId=${e.ExerciseId}" class="btn btn-info">📖 View Notes</a>
                            </div>
                        </div>
                    </div>`;
            });
            dayDiv.innerHTML += muscleBlock;
        }
        exerciseContainer.appendChild(dayDiv);
    }
}

/* ---------- Foods ---------- */
function renderFoods() {
    const foodsByDay = groupBy(assignedFoods, f => f.DayOfWeek);
    const foodContainer = document.getElementById('food-container');
    foodContainer.innerHTML = ''; // Clear existing content

    if (assignedFoods.length === 0) {
        // This case is already handled in the Razor view.
        return;
    }

    for (const [day, foods] of Object.entries(foodsByDay)) {
        const dayDiv = document.createElement('div');
        dayDiv.className = 'custom-card mb-4';
        dayDiv.innerHTML = `
            <div class="day-header">
                <span>🥗 ${dayNames[day] ?? `Day ${day}`}</span>
                <span>Nutrition Plan</span>
            </div>`;

        const foodsByMeal = groupBy(foods, f => f.MealNumber);
        for (const [mealNumber, mealFoods] of Object.entries(foodsByMeal)) {
            let mealHtml = `<div class="meal-block">
                <h5>Meal ${mealNumber}</h5>
                <table class="table-custom mb-3">
                    <thead>
                        <tr><th>Food</th><th>Quantity (g)</th><th>Servings</th><th>Notes</th></tr>
                    </thead>
                    <tbody>`;
            mealFoods.forEach(f => {
                mealHtml += `<tr>
                    <td>${f.FoodName ?? ''}</td>
                    <td>${f.Quantity ?? ''} g</td>
                    <td>${f.NumberOfServings ?? ''} serving(s)</td>
                    <td>${f.Notes || 'None'}</td>
                </tr>`;
            });
            mealHtml += `</tbody></table></div>`;
            dayDiv.innerHTML += mealHtml;
        }
        foodContainer.appendChild(dayDiv);
    }
}

/* ---------- Video Modal Functions ---------- */
function openVideo(url) {
    const modal = document.getElementById("videoModal");
    const frame = document.getElementById("videoFrame");
    const videoTag = document.getElementById("videoTag");

    let embedUrl = "";
    let useIframe = true;

    if (!url) {
        alert("No video URL provided.");
        return;
    }

    // Reset both elements
    frame.src = "";
    videoTag.pause();
    videoTag.src = "";
    frame.style.display = "none";
    videoTag.style.display = "none";

    // YouTube
    if (url.includes("youtube.com/watch?v=")) {
        const videoId = url.split("v=")[1].split("&")[0];
        embedUrl = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
    } else if (url.includes("youtu.be/")) {
        const videoId = url.split("youtu.be/")[1].split("?")[0];
        embedUrl = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
    }
    // Vimeo
    else if (url.includes("vimeo.com/")) {
        const videoId = url.split("/").pop();
        embedUrl = `https://player.vimeo.com/video/${videoId}?autoplay=1`;
    }
    // Direct video files
    else if (url.match(/\.(mp4|webm|ogg)$/i)) {
        useIframe = false;
        videoTag.src = url;
        videoTag.style.display = "block";
        videoTag.autoplay = true;
        videoTag.load();
        videoTag.play().catch(error => {
            console.error("Autoplay failed:", error);
            // Optionally, show a message to the user that autoplay was prevented
        });
    }
    // Default to iframe, assuming it's a URL that can be embedded
    else {
        embedUrl = url + (url.includes("?") ? "&" : "?") + "autoplay=1";
    }

    if (useIframe) {
        frame.src = embedUrl;
        frame.style.display = "block";
    }

    modal.style.display = "flex";
}

function closeVideo() {
    const modal = document.getElementById("videoModal");
    const frame = document.getElementById("videoFrame");
    const videoTag = document.getElementById("videoTag");

    frame.src = ""; // Stop iframe video
    videoTag.pause(); // Pause HTML5 video
    videoTag.src = ""; // Clear HTML5 video source
    modal.style.display = "none";
}

// Optional: Close when clicking outside video
window.onclick = function (event) {
    const modal = document.getElementById("videoModal");
    if (event.target === modal) {
        closeVideo();
    }
};

/* ---------- Tab Switching Logic (New) ---------- */
function setupTabSwitching() {
    const tabButtons = document.querySelectorAll('.plan-tab-btn');
    const planContents = document.querySelectorAll('.plan-content');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            // Remove 'active' from all buttons and contents
            tabButtons.forEach(btn => btn.classList.remove('active'));
            planContents.forEach(content => content.classList.remove('active'));

            // Add 'active' to the clicked button
            button.classList.add('active');

            // Show the corresponding content
            const targetTabId = button.dataset.tab + '-tab';
            document.getElementById(targetTabId).classList.add('active');
        });
    });

    // Initial render based on the default active tab
    const initialActiveTab = document.querySelector('.plan-tab-btn.active');
    if (initialActiveTab) {
        const targetTabId = initialActiveTab.dataset.tab + '-tab';
        document.getElementById(targetTabId).classList.add('active');
    }
}


// Call render functions and setup tab switching on page load
document.addEventListener('DOMContentLoaded', () => {
    renderExercises();
    renderFoods();
    setupTabSwitching();
});