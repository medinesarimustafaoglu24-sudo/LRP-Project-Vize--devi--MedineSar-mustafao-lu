



async function loadPage(contentName) {
    const mainContent = document.getElementById('main-container');

    
    try {
        const response = await fetch(`/pages/${contentName}.html`);
        const html = await response.text();
        mainContent.innerHTML = html;

        
        if (contentName === 'lab-list') fetchLabs();
        if (contentName === 'pc-assign') prepareAssignForm();

    } catch (error) {
        mainContent.innerHTML = "<h3>Sayfa yüklenirken hata oluştu!</h3>";
    }
}