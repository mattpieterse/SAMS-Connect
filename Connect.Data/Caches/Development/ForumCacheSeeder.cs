using System.Collections.Concurrent;
using Connect.Data.Models;

namespace Connect.Data.Caches.Development;

/// <summary>
/// Development-only seeder factory for the in-memory cache.
/// </summary>
/// <remarks>
/// NOT IMPORTANT
/// </remarks>
public static class ForumCacheSeeder
{
#region Factory

    public static ConcurrentDictionary<DateTime, IForumBroadcast> Init() {
        var store = new ConcurrentDictionary<DateTime, IForumBroadcast>();
        var currentTime = DateTime.UtcNow;

        /* Attribution: Seeder data sets
         * - (ChatGPT) https://openai.com/
         */
        var seeded = new IForumBroadcast[] {
            new ForumLetter {
                Heading = "Planned Power Interruption: Central District Upgrade",
                Content =
                    "Attention Central District residents: A **critical, scheduled power interruption** is set for this Friday, October 17th, from 9:00 AM to 3:00 PM. This necessary outage will facilitate major upgrades to the primary substation infrastructure serving your area. Our **Energy Department** engineers will be replacing aging transformers and installing smart grid technology to enhance long-term reliability and reduce future unscheduled outages. During this six-hour window, all electrical service will be temporarily suspended. We strongly recommend unplugging sensitive electronic equipment beforehand. Residents requiring life support equipment should ensure backup power systems are operational. We apologize for the inconvenience and appreciate your understanding as we work to modernize our electrical network. The work is crucial for supporting the growing energy demands and improving the overall resilience of the municipal power supply. For emergency contacts during the outage, please consult the dedicated Energy Department hotline.",
                Category = MunicipalDepartment.Energy,
                CreatedAt = currentTime.AddDays(-2),
                UpdatedAt = currentTime.AddDays(-2)
            },
            new ForumEvent {
                Heading = "Public Town Hall: Roads & Stormwater Capital Plan",
                Content =
                    "All citizens are invited to attend the **Annual Roads and Stormwater Planning Meeting** this coming Monday, October 20th, at 6:30 PM in the City Hall Auditorium. This vital forum is where the **Roads and Stormwater Department** will present proposed capital projects for the upcoming fiscal year, covering essential areas like road resurfacing, bridge maintenance, and major storm drain expansion projects. The agenda includes a detailed look at the new municipal flood mitigation strategy. Following the presentation, there will be an extensive Q&A session, providing a direct opportunity for residents to voice concerns and offer feedback that will directly influence the prioritization of projects. Your **community input is critical** to ensuring our infrastructure investments are aligned with the actual needs of the people we serve. Detailed project summaries will be available for review at the meeting.",
                Category = MunicipalDepartment.RoadsAndStormwater,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(5),
                UpdatedAt = currentTime.AddDays(5)
            },
            new ForumLetter {
                Heading = "Western Sector Water Main Repaired Ahead of Schedule",
                Content =
                    "Excellent news! The emergency **water main repair** initiated yesterday in the Western Sector has been successfully completed and tested ahead of the projected timeline. The **Water and Sanitation Department** crews worked tirelessly overnight to fully isolate and replace a critical segment of the aging distribution line that caused the disruption. As of 1:00 PM today, full water pressure has been completely restored to all affected properties. We appreciate the immense patience and cooperation demonstrated by all residents during this necessary repair. Following the interruption, you may notice some temporary cloudiness in your tap water due to trapped air or sediment. We recommend running your cold water tap for a few minutes until it runs clear. If the problem persists past 24 hours, please immediately contact our non-emergency line. We are committed to maintaining the highest quality of service and potable water for all citizens.",
                Category = MunicipalDepartment.WaterAndSanitation,
                CreatedAt = currentTime.AddHours(-6),
                UpdatedAt = currentTime.AddHours(-6)
            },
            new ForumEvent {
                Heading = "Road Closure: Emergency Waste Removal & Cleanup",
                Content =
                    "URGENT PUBLIC SAFETY ALERT: A major section of **Main Street**, between Oak Avenue and Elm Street, is currently closed for all traffic due to an unforeseen incident requiring immediate **Waste Management Department** response. The closure is effective immediately and is anticipated to last for the next 48 hours. This action is necessary to safely manage and remove a spill of disruptive commercial waste and to conduct emergency clean-up operations. **Waste Management Department** crews are on-site with specialized equipment to expedite the process and restore public access as quickly as possible. All motorists are strongly advised to follow the official detour signs posted—alternative routes via Central Boulevard are recommended. We prioritize the health and safety of our community, and this temporary closure is essential to prevent environmental contamination. Regular updates on the progress and estimated time of reopening will be provided via this channel.",
                Category = MunicipalDepartment.WasteManagement,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(-1),
                UpdatedAt = currentTime.AddDays(-1)
            },
            new ForumEvent {
                Heading = "Grand Opening: New Customer Service Center",
                Content =
                    "Mark your calendars! The **Water and Sanitation Department** is thrilled to announce the official **Grand Opening Ceremony** for our **New Municipal Service Center** this Saturday, October 18th, at 10:00 AM. Located conveniently at 450 City Circle, this modern facility is designed to significantly improve service delivery and accessibility for all water-related citizen needs. The new center will feature dedicated stations for customer support, including in-person bill payment, assistance with consumption inquiries, and processing of new service applications. The inaugural event will include a ribbon-cutting ceremony, an address from the Mayor and the Department Director, and guided tours to showcase the technology. Staff will be on hand to answer questions about water conservation programs and the city's future quality initiatives. This center represents a major investment in public service, making interactions with the department as efficient and transparent as possible. We look forward to welcoming the entire community to this milestone event!",
                Category = MunicipalDepartment.WaterAndSanitation,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(2),
                UpdatedAt = currentTime.AddDays(2)
            },
            new ForumEvent {
                Heading = "Load Shedding Relief Workshop and Q&A",
                Content =
                    "Join the **Energy Department** for a free **Load Shedding Readiness Workshop** next Tuesday, October 21st, at the Municipal Events Hall at 7:00 PM. This essential community session will focus on practical steps residents can take to minimize the impact of rolling blackouts. Topics covered will include the safe use of alternative power sources (inverters, solar panels, and generators), protecting home electronics from power surges, and interpreting the new municipal load shedding schedules. The workshop will feature a presentation by the Chief Engineer, followed by an open Q&A forum. Attendees will receive a printed Load Shedding Survival Guide. We encourage all homeowners and business owners to attend this event to gain vital knowledge and interact directly with the team responsible for managing and distributing our power supply. RSVP is recommended due to limited seating.",
                Category = MunicipalDepartment.Energy,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(6),
                UpdatedAt = currentTime.AddDays(6)
            },
            new ForumLetter {
                Heading = "New Two-Bag Recycling Initiative to Commence",
                Content =
                    "The **Waste Management Department** is excited to announce the launch of a **new, simplified two-bag recycling system** beginning November 1st. This initiative aims to dramatically increase the city's recycling rate and reduce landfill volume. Every household will receive a free introductory package containing two distinct coloured bags: one for mixed recyclables (paper, plastic, cans) and one for garden/organic waste. Normal refuse collection schedules will remain in place, but the new bags must be placed out separately. Please note that unseparated recyclables will not be collected after the launch date. Detailed instructional pamphlets outlining what can and cannot be placed in each bag are being mailed to all municipal account holders this week. We urge all residents to embrace this change and partner with the **Waste Management Department** to build a cleaner, greener city.",
                Category = MunicipalDepartment.WasteManagement,
                CreatedAt = currentTime.AddDays(-5),
                UpdatedAt = currentTime.AddDays(-5)
            },
            new ForumLetter {
                Heading = "New Metro Police Patrols Launched in Eastern Suburb",
                Content =
                    "In response to community feedback, the **Safety and Security Department** is immediately launching enhanced Metro Police visibility patrols across the Eastern Suburbs. This operation involves a dedicated fleet of patrol vehicles and an increased deployment of officers focusing on high-traffic retail areas and residential streets during evening hours. The objective is to proactively deter crime, enforce traffic regulations, and improve overall public safety perception. We remind residents to remain vigilant and report all suspicious activity directly to the Metro Police emergency line rather than non-emergency channels. This initiative forms part of the ongoing commitment by the **Safety and Security Department** to ensure the protection and well-being of all citizens and their property throughout the municipal area. We believe this increased presence will significantly enhance neighbourhood security.",
                Category = MunicipalDepartment.SafetyAndSecurity,
                CreatedAt = currentTime.AddDays(-3),
                UpdatedAt = currentTime.AddDays(-3)
            },
            new ForumEvent {
                Heading = "Future City Vision Public Consultation Session",
                Content =
                    "The **Planning and Development Department** invites all stakeholders to a crucial public consultation session on the **Draft 2040 Spatial Development Framework**. This is a once-in-a-decade opportunity to shape the future land use, density, and character of our city. The session will be held next Wednesday, October 22nd, at the Municipal Chambers at 5:00 PM. The presentation will cover proposed zoning changes, new requirements for urban infill housing, and strategies for preserving ecological corridors. Community input is essential, as the framework will guide all development approvals for the next twenty years. We encourage developers, architects, environmental groups, and residents to review the draft document online and attend the session to contribute to a sustainable and equitable city design.",
                Category = MunicipalDepartment.PlanningAndDevelopment,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(7),
                UpdatedAt = currentTime.AddDays(7)
            },
            new ForumLetter {
                Heading = "Winter Hours for Parks and Libraries Begin",
                Content =
                    "The **Community Services Department** announces that the transition to winter operating hours for all municipal parks, public swimming pools, and libraries will take effect on November 1st. All public parks will now close one hour earlier at 6:00 PM daily to coincide with earlier sunsets, and all branch libraries will reduce Saturday hours to 9:00 AM – 1:00 PM. These seasonal adjustments are necessary to ensure the safety of patrons and optimize operational efficiency during the colder months. Detailed, updated schedules for all facilities are posted on the municipal website under the 'Community Services' section. We thank the community for their understanding and encourage residents to continue utilizing our facilities during the revised hours.",
                Category = MunicipalDepartment.CommunityServices,
                CreatedAt = currentTime.AddDays(-10),
                UpdatedAt = currentTime.AddDays(-10)
            },
            new ForumLetter {
                Heading = "Rates & Tariffs Due Date Extension: October",
                Content =
                    "The **Finance and Revenue Department** has approved a **five-day extension** for the payment of all municipal rates, taxes, and service tariffs originally due on October 31st. The new deadline is now Monday, November 5th. This extension is being granted due to recent technical difficulties experienced by the online payment portal. We apologize for the inconvenience and hope this grace period assists residents and businesses in meeting their obligations without penalty. Please note that this extension applies only to the October cycle. Payments can still be made in person at any municipal service center or via the updated online portal, which is now fully operational. The **Finance and Revenue Department** remains committed to efficient and transparent revenue collection to fund all essential services.",
                Category = MunicipalDepartment.FinanceAndRevenue,
                CreatedAt = currentTime.AddDays(-4),
                UpdatedAt = currentTime.AddDays(-4)
            },
            new ForumEvent {
                Heading = "Road Resurfacing Project: Phase 3 Begins in Southern Sector",
                Content =
                    "Heads up, Southern Sector residents! Phase 3 of the municipal road resurfacing project is set to begin next Monday, October 28th, focusing on the entirety of Acacia Drive. The **Roads and Stormwater Department** estimates that this phase, which includes full excavation, base repair, and new asphalt laying, will take approximately three weeks to complete, weather permitting. Temporary road closures and detours will be clearly marked, and no street parking will be allowed during working hours (7:00 AM to 5:00 PM). This work is crucial for improving driving safety and extending the lifespan of our essential infrastructure. We appreciate your patience and cooperation as the **Roads and Stormwater Department** works efficiently to complete this vital upgrade to the community’s transportation network.",
                Category = MunicipalDepartment.RoadsAndStormwater,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(13),
                UpdatedAt = currentTime.AddDays(13)
            },
            new ForumLetter {
                Heading = "Mandatory Level 1 Water Restrictions Effective Immediately",
                Content =
                    "Due to consistently low reservoir levels following the dry season, the **Water and Sanitation Department** is enacting **Mandatory Level 1 Water Restrictions**, effective immediately. Residents are prohibited from watering gardens and lawns between 6:00 AM and 6:00 PM and are restricted to using only hand-held hoses or buckets for outdoor washing. Commercial car washes, golf courses, and industrial users must reduce consumption by 10%. These measures are necessary to safeguard our strategic water reserves and prevent the need for more drastic restrictions later in the year. The **Water and Sanitation Department** urges all citizens to adhere strictly to these rules. Fines will be issued for non-compliance. We thank the public for their sacrifice and partnership in preserving our precious water resources.",
                Category = MunicipalDepartment.WaterAndSanitation,
                CreatedAt = currentTime.AddHours(-1),
                UpdatedAt = currentTime.AddHours(-1)
            },
            new ForumLetter {
                Heading = "Illegal Connections Warning and Amnesty Period",
                Content =
                    "The **Energy Department** is issuing a final warning regarding the dangers and illegality of unauthorized electrical connections. These connections cause significant load instability, lead to frequent outages, and pose extreme fire and electrocution hazards to the community. However, the Department is simultaneously announcing a **30-day amnesty period** starting today. During this time, residents can report and regularize illegal connections without facing immediate legal action or heavy fines. Our technical team will safely disconnect the illegal connections and advise on the proper, legal procedures for acquiring service. After this period, the **Energy Department** will launch aggressive auditing and prosecution measures. Please use this opportunity to protect your neighborhood and ensure legal access to electricity.",
                Category = MunicipalDepartment.Energy,
                CreatedAt = currentTime.AddDays(-7),
                UpdatedAt = currentTime.AddDays(-7)
            },
            new ForumEvent {
                Heading = "Bridge Safety Inspection and Temporary Closure",
                Content =
                    "A vital **Structural Safety Inspection** is scheduled for the Riverbank Bridge connecting the Northern and Western districts next Saturday, October 26th, from 6:00 AM to 12:00 PM. The **Roads and Stormwater Department** requires a complete temporary closure of the bridge for both vehicles and pedestrians to allow engineers to use sensitive sonic equipment and conduct a detailed assessment of its structural integrity. This proactive maintenance ensures the bridge remains safe and fully compliant with national safety standards. Detour routes via the Ring Road will be clearly marked. Please adjust your travel plans to avoid this area during the inspection period. The **Roads and Stormwater Department** apologizes for the inconvenience but prioritizes public safety and the long-term viability of our critical transport infrastructure.",
                Category = MunicipalDepartment.RoadsAndStormwater,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(11),
                UpdatedAt = currentTime.AddDays(11)
            },
            new ForumEvent {
                Heading = "Hazardous Waste Drop-Off Day",
                Content =
                    "The **Waste Management Department** is hosting its quarterly **Hazardous Waste Drop-Off Day** this Saturday, October 19th, from 9:00 AM to 3:00 PM at the Municipal Depot (Entrance B). This is a safe and legal way for residents to dispose of materials that cannot go into general waste, including old paint, solvents, oil, expired medicines, batteries, and fluorescent tubes. Trained staff will be on hand to ensure proper segregation and environmentally responsible disposal. Please do not bring commercial or industrial waste. This free service is a key initiative to prevent environmental pollution and protect the health of our water table. Join the **Waste Management Department** in making a responsible choice for our environment. Identification proving municipal residency will be required.",
                Category = MunicipalDepartment.WasteManagement,
                Location = MunicipalProvincial.Gp,
                CreatedAt = currentTime.AddDays(4),
                UpdatedAt = currentTime.AddDays(4)
            }
        };

        foreach (var item in seeded)
            store[item.CreatedAt] = item;

        return store;
    }

#endregion
}
