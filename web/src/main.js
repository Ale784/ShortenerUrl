let BaseUrl = "http://localhost:5120/shortener";

const button = document.getElementById("send");
const originalUrl = document.getElementById("longUrl");
const errorMessages = document.getElementById("errorMessages");
const main = document.getElementsByTagName("main")[0]

button.addEventListener("click", HandleForm);

async function HandleForm(e) {
  e.preventDefault();

  try {
    const { shortUrl, redirectUrl } = await PostData(originalUrl);
   
    if(shortUrl && redirectUrl != undefined) {
      displayResult(shortUrl, redirectUrl)
    }

  } catch (error) {
    console.error("Error handling form:", error)
  }

}

function displayResult(shortUrl, redirectUrl) {
  const resultContainer = document.createElement("div");
  resultContainer.id = "shortener-container";
  resultContainer.className = "w-full bg-white p-4 flex flex-col gap-6";

  const shortUrlContainer = document.createElement("div");
  shortUrlContainer.className = "flex justify-between w-full";

  const shortUrlElement = document.createElement("div");
  shortUrlElement.innerHTML = `
    <span class="text-xl font-semibold text-blue-600">Shortener Url: </span>
    <a href="${shortUrl}" target="_blank">${shortUrl}</a>
  `;

  const copyButton = document.createElement("button");
  copyButton.textContent = "Copy Text";
  copyButton.className = "py-2.5 px-5 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-200 dark:focus:ring-gray-700 dark:bg-gray-800 dark:text-gray-400 dark:border-gray-600 dark:hover:text-white dark:hover:bg-gray-700"
  copyButton.addEventListener("click", () => copyToClipboard(shortUrl));

  shortUrlContainer.appendChild(shortUrlElement);
  shortUrlContainer.appendChild(copyButton);

  const longUrlElement = document.createElement("div");
  longUrlElement.innerHTML = `
    <span class="text-xl font-semibold text-blue-600">Long Url: </span>
    <a href="${redirectUrl}" target="_blank">${redirectUrl}</a>
  `;

  resultContainer.appendChild(shortUrlContainer);
  resultContainer.appendChild(longUrlElement);

  main.appendChild(resultContainer);
}


function copyToClipboard(url){
    navigator.clipboard.writeText(url)
}

function isValidUrl(string) {
    try {
      new URL(string);
      return true;
    } catch (err) {
      return false;
    }
  }

function GetLongUrl(url) {

  const obj = {
    longUrl: "",
    error: [],
  };

  if (url == "") {
    obj.error.push("Url cannot be empty");
  }

  if(!isValidUrl(url))
  {
    obj.error.push("Invalid url");
  }

  obj.longUrl = url;

  return obj;
}

async function PostData(inputUrl, e) {
  try {

    const url = GetLongUrl(inputUrl.value);
    const { error } = url;

    if (error.length > 0) {
      displayErrorMessage(error)
      throw new Error("Validation error");
    }

    const { longUrl } = url;

    const data = JSON.stringify({
      OriginalUrl: longUrl,
    });

    const settings = {
      method: "POST",
      headers: {
        Accept: "application/json",
        "Content-Type": "application/json",
      },
      body: data,
    };

    const response = await fetch(BaseUrl, settings);
    const result = await response.json();

    return result;

} catch (error) {
    console.error("error:", error);
    return  error;
  }
}

function displayErrorMessage(error){
    let p = document.createElement("p");
    p.className = "text-white"

    error.forEach((err) => {
      p.textContent = err;
      errorMessages.appendChild(p);
    });

    setTimeout(() => {
      p.remove();
    }, 1000);

}




