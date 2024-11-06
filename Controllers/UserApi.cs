using AboHelmy_Buisness;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Abu_HelmyWebAPI.Controllers
{
    [ApiController]
    [Route("api/abu_helmy/user")]
    public class UserApi:ControllerBase
    {

        [HttpGet("All",Name = "GetAllUser")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable< DataTable>> GetAllUser()
        {
            DataTable users =  clsUser.GetAllUsers();
            if (users == null)
                return NotFound("not found");

            return Ok(users);
        }

        [HttpPost(Name ="AddNewUswe")]
        [ProducesResponseType (StatusCodes.Status201Created)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public ActionResult<clsUser> AddNewUser(clsUser newUser)
        {
            if (string.IsNullOrEmpty(newUser.UserName) || string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest("bad request");
            }
            if (newUser.Save())
            {
                var user = clsUser.FindByUserID(newUser.UserID);
                return CreatedAtRoute("AddNewUser", user);
            }
            return BadRequest("bad requset");
        }


        [HttpPut("{id}",Name = "UpdateUser")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public ActionResult<clsUser> UpdateUser(int id , clsUser updateUser)
        {
            if(string.IsNullOrEmpty(updateUser.UserName) || string.IsNullOrEmpty(updateUser.Password))
            {
                return BadRequest("bad request");
            }
            var user = clsUser.FindByUserID(id);
            user.UserName = updateUser.UserName;
            user.Password = updateUser.Password;
            user.IsActive = updateUser.IsActive;
            if(user.Save())
            {
                return Ok(user);
            }
            return NotFound("not found");
        }


        [HttpGet("{id}", Name = "GetUserByID")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public ActionResult<clsUser> GetUserByID(int id)
        {
            if (id < 0)
            {
                return BadRequest("bad request");
            }
            var user = clsUser.FindByUserID(id);
            if (user == null)
            {
                return Ok (user);
            }
            return NotFound("not found");
        }


        [HttpGet("{name,password}",Name ="GetUserByNameAndPassword")]
        [ProducesResponseType (StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status404NotFound)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public ActionResult<clsUser> GetUserByNameAndPassword(string name, string password)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                return BadRequest("bad request");
            } 
            var user = clsUser.FindByUsernameAndPassword(name, password);
            if(user == null)
            {
                return Ok (user);
            }
            return NotFound("not found");
        }


        [HttpDelete("{id}",Name = "DeleteUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteUser(int id)
        {
            if (id < 0)
            {
                return BadRequest("bad request");
            }
            if(clsUser.DeleteUser(id))
            {
                return Ok("delete user");
            }
            return NotFound("not found");
        }



    }
}
