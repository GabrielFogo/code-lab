const radioButtons = document.querySelectorAll('input[type="radio"]');
const submitButton = document.getElementById("submitButton");

// Função para mudar a cor do botão
function changeButtonColor() {
    submitButton.style.borderColor = "#3bce07"; // Cor quando uma opção é selecionada
    submitButton.style.color = "#3bce07";
    submitButton.style.boxShadow = "inset 0 0 7px 1px #3bce07"; // Sombra interna verde
    submitButton.textContent = "Próximo";

    // Adiciona o ícone de tick ao botão (se ainda não estiver presente)
    const tickIcon = document.createElement("i");
    tickIcon.classList.add("fas", "fa-check"); // Classes Font Awesome para o ícone de checkmark
    tickIcon.style.marginLeft = "10px"; // Espaçamento entre o texto e o ícone
    submitButton.appendChild(tickIcon); // Adiciona o ícone ao botão
}

// Adiciona o evento 'change' a cada radio button
radioButtons.forEach((radio) => {
    radio.addEventListener("change", function () {
        // Se um radio button for selecionado, mude a cor do botão
        if (this.checked) {
            changeButtonColor();
        }
    });
});
