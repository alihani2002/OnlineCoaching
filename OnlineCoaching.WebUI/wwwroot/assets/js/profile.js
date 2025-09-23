const dayNames = {
    1: "First Day", 2: "Second Day", 3: "Third Day", 4: "Fourth Day",
    5: "Fifth Day", 6: "Sixth Day", 7: "Seventh Day",
    8: "1,2,3,4,5,6 & 7", 9: "1,2,3", 10: "4,5,6,7"
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
const exercisesByDay = groupBy(assignedExercises, e => e.DayOfWeek);
const exerciseContainer = document.getElementById('exercise-container');

for (const [day, exercises] of Object.entries(exercisesByDay)) {
    const dayDiv = document.createElement('div');
    dayDiv.className = 'custom-card mb-4';
    dayDiv.innerHTML = `
        <div class="day-header">
            <span>🏋 ${dayNames[day] ?? day}</span>
            <span>Exercises</span>
        </div>`;

    const exercisesByMuscle = groupBy(exercises, e => e.MuscleName ?? "Other");
    for (const [muscle, muscleExercises] of Object.entries(exercisesByMuscle)) {
        let muscleBlock = `<div class="muscle-header">💪 ${muscle}</div>`;
        muscleExercises.forEach(e => {
            muscleBlock += `
                <div class="exercise-card">
                    <div class="exercise-details">
                        <div><strong>${e.NameOfExercise ?? ''}</strong></div>
                        <div>Sets: ${e.Sets ?? ''}</div>
                        <div>Reps: ${e.Reps ?? ''}</div>
                        <div>Notes: ${e.Notes ?? ''}</div>
                        <div class="exercise-actions">
                            <a href="/ExerciseSheetLogs/Create?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-warning ">➕ Log</a>
                            <a href="/ExerciseSheetLogs/ClientExerciseLogs?exerciseId=${e.ExerciseId}&clientId=${clientId}" class="btn btn-info ">📊 Logs</a>
                            <a href="/ExerciseNote/Create?exerciseId=${e.ExerciseId}" 
                               class="btn btn-sm btn-primary mt-2">✏️ Add Note</a>
                               <a href="/ExerciseNote/Index?exerciseId=${e.Id}" 
                               class="btn btn-sm btn-info mt-2">
                               📖 View Notes
                            </a>

                        </div>
                    </div>
                    <div class="exercise-image">
                        ${e.ImageUrl
                    ? `<a href="javascript:void(0);" onclick="openVideo('${e.VideoUrl ?? ''}')">
                       <img src="${e.ImageUrl}" class="main-img" alt="${e.NameOfExercise}" />
                        </a>`
                    : `<span class="text-muted p-3">No Image</span>`}
                    </div>
                </div>`;
        });
        dayDiv.innerHTML += muscleBlock;
    }
    exerciseContainer.appendChild(dayDiv);
}

/* ---------- Foods ---------- */
const foodsByDay = groupBy(assignedFoods, f => f.DayOfWeek);
const foodContainer = document.getElementById('food-container');

for (const [day, foods] of Object.entries(foodsByDay)) {
    const dayDiv = document.createElement('div');
    dayDiv.className = 'custom-card mb-4';
    dayDiv.innerHTML = `
        <div class="day-header">
            <span>🥗 ${dayNames[day] ?? day}</span>
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
                <td>${f.Notes ?? ''}</td>
            </tr>`;
        });
        mealHtml += `</tbody></table></div>`;
        dayDiv.innerHTML += mealHtml;
    }
    foodContainer.appendChild(dayDiv);
}

function openVideo(url) {
    const modal = document.getElementById("videoModal");
    const frame = document.getElementById("videoFrame");
    const videoTag = document.getElementById("videoTag");

    let embedUrl = "";
    let useIframe = true;

    if (!url) return;

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
        frame.style.display = "none";

        // Force autoplay
        videoTag.autoplay = true;
        videoTag.load();
        videoTag.play();
    }
    // Default → treat as iframe
    else {
        embedUrl = url + (url.includes("?") ? "&" : "?") + "autoplay=1";
    }

    if (useIframe) {
        frame.src = embedUrl;
        frame.style.display = "block";
        videoTag.style.display = "none";
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

// Optional: Close when clicking outside video
window.onclick = function (event) {
    const modal = document.getElementById("videoModal");
    if (event.target === modal) {
        closeVideo();
    }
};
