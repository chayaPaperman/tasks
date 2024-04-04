const name = document.getElementById('name');
const password = document.getElementById('password');
const uri = '/login'

function handleCredentialResponse(response) {
    const responsePayload = decodeJwtResponse(response.credential);
    const user = {
        id: 0,
        name: responsePayload.name,
        password: responsePayload.email
    }
    console.log(user);
    login(user);
}
function decodeJwtResponse(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}

const sendToLogin = () => {
    const user = {
        Id: 0,
        Name: name.value,
        password: password.value
    }
    login(user);

}
const login = (user) => {
    fetch(uri, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    })
        .then(res => {
            if (res.status === 401)
                throw new Error();
            else
                return res.json();
        })
        .then(data => {
            localStorage.setItem("tasksToken", data)
        })
        .then(res => window.location.href = "../index.html")
        .catch(error => alert("we don't know you, enter again"))
        .finally(() => {
            name.value = '';
            password.value = ''
        })
}
// postman
(function (p, o, s, t, m, a, n) {
    !p[s] && (p[s] = function () {
        (p[t] || (p[t] = [])).push(arguments);
    });
    !o.getElementById(s + t) && o.getElementsByTagName("head")[0].appendChild((
        (n = o.createElement("script")),
        (n.id = s + t), (n.async = 1), (n.src = m), n
    ));
}(window, document, "_pm", "PostmanRunObject", "https://run.pstmn.io/button.js"));
