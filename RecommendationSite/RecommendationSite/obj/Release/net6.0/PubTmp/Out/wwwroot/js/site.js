window.onload = function() {
    document.getElementById('themer').addEventListener('click', () => {
        const current = document.documentElement.getAttribute('data-bs-theme')
        const inverted = current == 'dark' ? 'light' : 'dark'
        document.documentElement.setAttribute('data-bs-theme', inverted)
        document.getElementById('nav').setAttribute('data-bs-theme', current)
    })
}