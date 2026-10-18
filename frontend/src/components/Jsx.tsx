function ProfileCard() {
  return
  <>
    <div class="card">
      <img src="pic.jpg"/>
      <h2>{ isOnline ? "Online" : "Offline"}</h2>
      <p style={{"color": "red"}}>Status</p>
    </div>
    <button onClick={handleClick}>Click</button>
    {items.map(item => <li key={item.id}>{item.name}</li>)}
     
    </>
}