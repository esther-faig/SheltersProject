function UserCard()
{
    const user = {
        id: 1,
  name: "Nechama",
  isOnline: true,
  tags: ["React", "JavaScript"]
}
    return <>
    <h2>{user.name}</h2>
    <p>{user.isOnline ? "מחובר" : "לא מחובר"}</p>
    <hr/>
    <div className="card" ><ul>{user.tags.map((tag,index) => (<li key={index}>{tag}</li>))}</ul></div>
    {/*4 6 9 */ }
    <div style={{ color: 'red', fontSize: 20 }}></div>
    </>

}
export default UserCard