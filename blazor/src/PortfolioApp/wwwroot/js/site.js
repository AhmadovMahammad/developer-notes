let salesChartInstance = null;

function initChart(salesData) {
    const ctx = document.getElementById('salesChart').getContext('2d');

    if (salesChartInstance !== null) {
        salesChartInstance.destroy();
    }

    salesChartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ["Jan", "Feb", "Mar", "Apr", "May"],
            datasets: [{
                label: 'Sales',
                data: salesData,
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
}