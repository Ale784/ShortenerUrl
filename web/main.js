const button = document.getElementById("send")
const input = document.getElementById("shortenerInput")
const inputOriginalUrl = document.getElementById("originalUrl")

let BaseUrl = "http://localhost:5120/shortener"

const data = {
    "OriginalUrl": inputOriginalUrl.value
}

const settings = {
    method: 'POST',
    headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
    },
    body: JSON.stringify(data)
}


button.addEventListener("click", async(e) => { 
    e.preventDefault();
    
    const { shortUrl } = await PostData();
    input.value = shortUrl

})


async function PostData(){

    try {
        let result = await fetch(BaseUrl, settings)
        let data = result.json();
        
        return data
           
    } catch (error) {
        console.error("error:", error)
        return error
    }
}
