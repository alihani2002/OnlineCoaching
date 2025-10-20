/* ---------- Config ---------- */
const assignedExercises = window.assignedExercises || [];
const assignedFoods = window.assignedFoods || [];
const clientId = window.clientId || 0;

/* ---------- Helpers ---------- */
function groupBy(array, keyFunc) {
    return array.reduce((result, item) => {
        const key = keyFunc(item);
        if (!result[key]) result[key] = [];
        result[key].push(item);
        return result;
    }, {});
}

function getDays(item) {
    if (Array.isArray(item.SelectedDays) && item.SelectedDays.length > 0) {
        return item.SelectedDays.map(d => parseInt(d));
    }
    if (item.DayOfWeek) {
        return [parseInt(item.DayOfWeek)];
    }
    return [];
}

/* ---------- Exercises ---------- */
function renderExercises() {
    const exerciseContainer = document.getElementById('exercise-container');
    exerciseContainer.innerHTML = '';

    if (assignedExercises.length === 0) return;

    // 🟢 Group all exercises by days first
    const exercisesByDays = groupBy(assignedExercises, e => {
        const days = getDays(e).sort((a, b) => a - b);
        return days.length ? days.join(', ') : 'N/A';
    });

    // 🟢 Sort day groups numerically (1–7)
    const sortedDays = Object.keys(exercisesByDays).sort((a, b) => {
        const firstA = parseInt(a.split(',')[0]) || 0;
        const firstB = parseInt(b.split(',')[0]) || 0;
        return firstA - firstB;
    });

    // 🟢 Loop through each day group
    for (const days of sortedDays) {
        const dayExercises = exercisesByDays[days];
        let dayBlock = `
            <div class="custom-card mb-4">
                <div class="day-header">
                    <span>📅 Days: ${days}</span>
                </div>`;

        // 🟢 Group exercises by muscle within each day
        const exercisesByMuscle = groupBy(dayExercises, e => e.MuscleName ?? "Other");

        for (const [muscle, muscleExercises] of Object.entries(exercisesByMuscle)) {
            dayBlock += `
                <div class="muscle-section mt-3">
                    <h5 class="text-warning mb-2">💪 ${muscle}</h5>`;

            muscleExercises.forEach(e => {
                dayBlock += `
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
                                <a href="/ExerciseSheetLogs/Create?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-warning">➕ Log</a>
                                <a href="/ExerciseSheetLogs/ClientExerciseLogs?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-info">📊 Logs</a>
                                <a href="/ExerciseNote/Create?exerciseId=${e.ExerciseId}" class="btn btn-primary">✏️ Add Note</a>
                                <a href="/ExerciseNote/Index?exerciseId=${e.ExerciseId}" class="btn btn-info">📖 View Notes</a>
                            </div>
                        </div>
                    </div>`;
            });

            dayBlock += `</div>`; // close muscle-section
        }

        dayBlock += `</div>`; // close day-card
        exerciseContainer.innerHTML += dayBlock;
    }
}

/* ---------- Foods ---------- */
function renderFoods() {
    const foodContainer = document.getElementById('food-container');
    foodContainer.innerHTML = '';

    if (assignedFoods.length === 0) return;

    // Group foods by meal number
    const foodsByMeal = groupBy(assignedFoods, f => f.MealNumber);

    for (const [mealNumber, mealFoods] of Object.entries(foodsByMeal)) {
        // Now group inside each meal by days
        const foodsByDays = groupBy(mealFoods, f => {
            const days = getDays(f).sort((a, b) => a - b);
            return days.length ? days.join(', ') : 'N/A';
        });

        for (const [days, dayFoods] of Object.entries(foodsByDays)) {
            let mealHtml = `<div class="custom-card mb-4">
                <div class="day-header">
                    <span>🍽 Meal ${mealNumber}</span>
                    <span>Days: ${days}</span>
                </div>
                <table class="table-custom mb-3">
                    <thead>
                        <tr><th>Food</th><th>Quantity (g)</th><th>Servings</th><th>Notes</th></tr>
                    </thead>
                    <tbody>`;

            dayFoods.forEach(f => {
                mealHtml += `<tr>
                    <td>${f.FoodName ?? ''}</td>
                    <td>${f.Quantity ?? ''} g</td>
                    <td>${f.NumberOfServings ?? ''} serving(s)</td>
                    <td>${f.Notes || 'None'}</td>
                </tr>`;
            });

            mealHtml += `</tbody></table></div>`;
            foodContainer.innerHTML += mealHtml;
        }
    }
}

/* ---------- Video Modal ---------- */
function openVideo(url) {
    const modal = document.getElementById("videoModal");
    const frame = document.getElementById("videoFrame");
    const videoTag = document.getElementById("videoTag");

    if (!url) {
        alert("No video URL provided.");
        return;
    }

    frame.src = "";
    videoTag.pause();
    videoTag.src = "";
    frame.style.display = "none";
    videoTag.style.display = "none";

    let embedUrl = "";
    let useIframe = true;

    if (url.includes("youtube.com/watch?v=")) {
        const videoId = url.split("v=")[1].split("&")[0];
        embedUrl = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
    } else if (url.includes("youtu.be/")) {
        const videoId = url.split("youtu.be/")[1].split("?")[0];
        embedUrl = `https://www.youtube.com/embed/${videoId}?autoplay=1&rel=0`;
    } else if (url.includes("vimeo.com/")) {
        const videoId = url.split("/").pop();
        embedUrl = `https://player.vimeo.com/video/${videoId}?autoplay=1`;
    } else if (url.match(/\.(mp4|webm|ogg)$/i)) {
        useIframe = false;
        videoTag.src = url;
        videoTag.style.display = "block";
        videoTag.autoplay = true;
        videoTag.load();
        videoTag.play().catch(err => console.error("Autoplay failed:", err));
    } else {
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

    frame.src = "";
    videoTag.pause();
    videoTag.src = "";
    modal.style.display = "none";
}

window.onclick = function (event) {
    const modal = document.getElementById("videoModal");
    if (event.target === modal) closeVideo();
};

/* ---------- Tab Switching ---------- */
function setupTabSwitching() {
    const tabButtons = document.querySelectorAll('.plan-tab-btn');
    const planContents = document.querySelectorAll('.plan-content');

    tabButtons.forEach(button => {
        button.addEventListener('click', () => {
            tabButtons.forEach(btn => btn.classList.remove('active'));
            planContents.forEach(content => content.classList.remove('active'));
            button.classList.add('active');
            document.getElementById(button.dataset.tab + '-tab').classList.add('active');
        });
    });

    const initialActiveTab = document.querySelector('.plan-tab-btn.active');
    if (initialActiveTab) {
        document.getElementById(initialActiveTab.dataset.tab + '-tab').classList.add('active');
    }
}

/* ---------- Init ---------- */
document.addEventListener('DOMContentLoaded', () => {
    renderExercises();
    renderFoods();
    setupTabSwitching();
});
