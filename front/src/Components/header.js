import TokenName from "../Components/token_constant_name";
import Cookies from 'js-cookie';
import './../assets/css/header.css'
import './../assets/css/animate.css'
import './../assets/css/all.min.css'
import './../assets/css/swiper.min.css'
import './../assets/css/lightcase.css'
import './../assets/css/style.css'
import './../assets/css/bootstrap.min.css'


const HeaderApp = () => {
    const token = Cookies.get(TokenName);

    function RemoveCookies() {
        Cookies.remove('token');
        // Cookies.remove('.AspNetCore.Identity.Application');
    }

    return (
        <header className="header" id="navbar">
            <div className="navbar-nav mainmenu">
                <ul>
                    {
                        token? 
                        <>
                            <li>
                                <a href="/games">Games</a>
                            </li>
                            <li>
                                <a href="/rating">Rating</a>
                            </li>
                            <li>
                                <a onClick={RemoveCookies} href="/login">Log Out</a>
                            </li>

                        </>
                        :
                        <>
                            <li>
                                <a href="/login">Login</a>
                            </li>
                            <li>
                                <a href="/register">Sign up</a>
                            </li>

                        </>
                    }
                    
                </ul>
            </div>           

        </header>
    )

}



export default HeaderApp;