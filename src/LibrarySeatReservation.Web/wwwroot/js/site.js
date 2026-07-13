// LibrarySeatReservation - 前端交互脚本

document.addEventListener('DOMContentLoaded', function () {
    // 时间槽选择（预约提交页）
    const timeSlotItems = document.querySelectorAll('.time-slot-item');
    const timeSlotInput = document.getElementById('selectedTimeSlot');

    if (timeSlotItems.length > 0 && timeSlotInput) {
        timeSlotItems.forEach(function (item) {
            item.addEventListener('click', function () {
                timeSlotItems.forEach(function (el) { el.classList.remove('selected'); });
                this.classList.add('selected');
                timeSlotInput.value = this.dataset.slot;
            });
        });
    }

    // 自动隐藏 alert（3秒后淡出）
    const alerts = document.querySelectorAll('.alert-auto-dismiss');
    alerts.forEach(function (alert) {
        setTimeout(function () {
            alert.style.opacity = '0';
            alert.style.transition = 'opacity 0.5s';
            setTimeout(function () { alert.remove(); }, 500);
        }, 3000);
    });
});
