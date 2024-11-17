import Navigation from '../components/NavigationBar.jsx';

const HomePage = () => {
    return (
        <>
            <section className="main_container">
                <div className="sub_container">
                    <h1 className="main_title">
                        This is a web app
                    </h1>
                </div>

                <div className="home_container">
                    <Navigation />
                </div>
            </section>
        </>
    )
};

export default HomePage;